using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HoursMarket.Data;
using HoursMarket.Dto;
using HoursMarket.Extension;
using HoursMarket.Helper;
using HoursMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HoursMarket.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/houroffers")]
    public class HoursController : ControllerBase
    {

        private readonly IHoursMarketRepo _repository;
        private readonly IMapper _mapper;
        private readonly IAuthorizer _authorizer;
        private readonly IEmailSender _emailProvider;

        public HoursController(IHoursMarketRepo repo, IMapper mapper, IAuthorizer authorizer, IEmailSender emailProvider)
        {
            _repository = repo;
            _mapper = mapper;
            _authorizer = authorizer;
            _emailProvider = emailProvider;

        }





        [HttpGet("checkunassigned")]
        [Authorize]
        public ActionResult CheckUnassigned()
        {



            var account = _repository.GetAccountById(this.GetUserId());


            if (account == null)
            {
                return Unauthorized();
            }


            int[] proj = ProjectParser.ParseProjects(account.CurrentProject);

            foreach (int project in proj)
            {
                if (project == (int)CurrentProject.Unassigned)
                {
                    return Ok(new { unassigned = true });
                }
            }




            return Ok(new { unassigned = false });

        }




        [HttpGet]
        [Authorize]
        public ActionResult GetAuthorizedHourOffers()
        {


            var account = _repository.GetAccountById(this.GetUserId());

            if (account == null)
            {
                return Unauthorized();
            }





            IEnumerable<HourOffer> allOffers = _repository.GetAllHourOffers();


            List<HourOffer> offers = new List<HourOffer>();




            int[] proj = ProjectParser.ParseProjects(account.CurrentProject);

            foreach (int project in proj)
            {
                offers.AddRange(allOffers.Where(x => x.Project == project));
            }
            offers.Sort((x, y) => DateTime.Compare(x.BeginDate, y.BeginDate));





            foreach (HourOffer offer in offers)
            {
                if (offer.AccountId == account.Id)
                {
                    offer.Owned = true;
                }
                else
                {
                    offer.Owned = false;
                }
            }

            return Ok(offers);
        }


        [HttpGet("myoffers")]
        [Authorize]
        public ActionResult GetMyOffers()
        {
            var account = _repository.GetAccountById(this.GetUserId());

            if (account == null)
            {
                return Unauthorized();
            }

            List<HourOffer> offers = _repository.GetAllHourOffers().Where(x => x.AccountId == account.Id).ToList();

            foreach (HourOffer offer in offers)
            {
                if (offer.AccountId == account.Id)
                {
                    offer.Owned = true;
                }
                else
                {
                    offer.Owned = false;
                }
            }

            return Ok(offers);
        }







        [HttpGet("{id}")]
        [Authorize]
        public ActionResult GetHourOfferById(int id)
        {
            var hourOffer = _repository.GetHourOfferById(id);
            var account = _repository.GetAccountById(this.GetUserId());

            if (account == null)
            {
                return Unauthorized();
            }






            if (hourOffer != null)
            {

                int[] projs = ProjectParser.ParseProjects(account.CurrentProject);

                for (int i = 0; i < projs.Length; i++)
                {
                    if (hourOffer.Project == projs[i])
                    {
                        break;
                    }


                    if (i == projs.Length - 1)
                    {
                        return Forbid();
                    }
                }

                if (hourOffer.AccountId == account.Id)
                {
                    hourOffer.Owned = true;
                }
                else
                {
                    hourOffer.Owned = false;
                }

                return Ok(hourOffer);
            }
            return NotFound();

        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateHourOffer(HourOfferDto offer)
        {
            var account = _repository.GetAccountById(this.GetUserId());


            if (account == null)
            {
                return Unauthorized();
            }

            if (!this.AuthorizeByCurrentProjects(new List<CurrentProject>() { CurrentProject.Innogy, CurrentProject.NFZ }, _authorizer))
            {
                return Forbid();
            }


            int[] projs = ProjectParser.ParseProjects(account.CurrentProject);

            for (int i = 0; i < projs.Length; i++)
            {
                if (offer.Project == projs[i])
                {
                    break;
                }


                if (i == projs.Length - 1)
                {
                    return Forbid();
                }
            }

            if (!Enum.IsDefined(typeof(CurrentProject), offer.Project))
            {
                return BadRequest();
            }



            var hourOffer = _mapper.Map<HourOffer>(offer);
            hourOffer.Project = offer.Project;
            hourOffer.Name = account.Name;
            hourOffer.AccountId = account.Id;

            if (hourOffer.EndDate.Ticks >= hourOffer.BeginDate.Ticks + TimeSpan.TicksPerDay || hourOffer.BeginDate.Ticks >= hourOffer.EndDate.Ticks || hourOffer.BeginDate < DateTime.Now)
            {
                return BadRequest();
            }

            //   hourOffer.BeginDate = hourOffer.BeginDate.ToUniversalTime();
            //  hourOffer.EndDate = hourOffer.EndDate.ToUniversalTime();

            _repository.CreateHourOffer(hourOffer);





            try
            {
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetHourOfferById), new { id = hourOffer.Id }, hourOffer);

        }



        [HttpDelete("takehouroffer/{id}")]
        [Authorize]
        public ActionResult TakeHourOffer(int id)
        {



            var hourOffer = _repository.GetHourOfferById(id);
            var account = _repository.GetAccountById(this.GetUserId());



            if (account == null)
            {
                return Unauthorized();
            }

            if (hourOffer != null)
            {

                int[] projs = ProjectParser.ParseProjects(account.CurrentProject);

                for (int i = 0; i < projs.Length; i++)
                {
                    if (hourOffer.Project == projs[i])
                    {
                        break;
                    }


                    if (i == projs.Length - 1)
                    {
                        return Forbid();
                    }
                }


                if (hourOffer.AccountId == account.Id)
                {
                    return BadRequest();
                }




                try
                {

                    _repository.DeleteHourOffer(hourOffer);
                    _repository.SaveChanges();
                }

                catch (DbUpdateConcurrencyException)
                {
                    return NotFound("Resource Already Deleted.");
                }



                List<ManagerEmail> emails = _repository.GetManagerEmailsByProject((CurrentProject)hourOffer.Project).ToList();

                string emailsFormatted = "";

                for (int i = 0; i < emails.Count; i++)
                {
                    emailsFormatted += emails[i].Email;
                    if (i == emails.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        emailsFormatted += ";";
                    }



                }

                emailsFormatted += ";" + _repository.GetAccountById(hourOffer.AccountId).Email;
                _emailProvider.SendEmail(emailsFormatted, "HourMarket Transaction", $"Dotyczy projektu: {Enum.GetName(typeof(CurrentProject), (CurrentProject)hourOffer.Project)}{Environment.NewLine}{account.Name} wziął godziny {hourOffer.BeginDate} do {hourOffer.EndDate} od {hourOffer.Name}");



                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteHourOffer(int id)
        {
            var hourOffer = _repository.GetHourOfferById(id);
            var account = _repository.GetAccountById(this.GetUserId());

            if (account == null)
            {
                return Unauthorized();
            }

            if (hourOffer != null)
            {
                if (hourOffer.AccountId != account.Id)
                {
                    return Forbid();
                }

                try
                {
                    _repository.DeleteHourOffer(hourOffer);
                    _repository.SaveChanges();
                }

                catch (DbUpdateConcurrencyException)
                {
                    return NotFound("Resource Already Deleted.");
                }
                return Ok();
            }

            return NotFound();
        }

        [HttpPatch("{id}")]
        [Authorize]
        public ActionResult UpdateHourOffer(int id, JsonPatchDocument<HourOfferDto> patch)
        {

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }



            var hourOffer = _repository.GetHourOfferById(id);
            var testOffer = new HourOffer();
            var account = _repository.GetAccountById(this.GetUserId());

            if (account == null)
            {
                return Unauthorized();
            }







            if (hourOffer != null)
            {
                if (hourOffer.AccountId != account.Id)
                {
                    return Forbid();
                }


                var testPatchCommand = _mapper.Map<HourOfferDto>(testOffer);
                var commandToPatch = _mapper.Map<HourOfferDto>(hourOffer);



                foreach (Operation o in patch.Operations)
                {

                    if (o.OperationType != OperationType.Replace)
                        return BadRequest();
                }



                try
                {
                    patch.ApplyTo(testPatchCommand);


                    int[] projs = ProjectParser.ParseProjects(account.CurrentProject);

                    for (int i = 0; i < projs.Length; i++)
                    {
                        if (testPatchCommand.Project == projs[i])
                        {
                            break;
                        }


                        if (i == projs.Length - 1)
                        {
                            return Forbid();
                        }
                    }

                    if (!Enum.IsDefined(typeof(CurrentProject), testPatchCommand.Project))
                    {
                        return BadRequest();
                    }



                    patch.ApplyTo(commandToPatch);
                }
                catch (ArgumentNullException)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return new BadRequestObjectResult(ModelState);
                }


                _mapper.Map(commandToPatch, hourOffer);

                if (hourOffer.EndDate.Ticks >= hourOffer.BeginDate.Ticks + TimeSpan.TicksPerDay || hourOffer.BeginDate.Ticks >= hourOffer.EndDate.Ticks || hourOffer.BeginDate < DateTime.Now)
                {
                    return BadRequest();
                }

                hourOffer.BeginDate = hourOffer.BeginDate;
                hourOffer.EndDate = hourOffer.EndDate;

                try
                {



                    _repository.SaveChanges();
                }

                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest();
                }

                return Ok();

            }
            return NotFound();
        }

    }
}

