using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Mvc;
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




        [HttpGet]
        [Authorize]
        public ActionResult GetAuthorizedHourOffers()
        {
            var account = _repository.GetAccountById(this.GetUserId());
            List<HourOffer> offers = _repository.GetAllHourOffers().Where(x => x.Project == account.CurrentProject).ToList();

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



            if (hourOffer != null)
            {
                if (hourOffer.Project != account.CurrentProject)
                {
                    return Forbid();
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
            var hourOffer = _mapper.Map<HourOffer>(offer);
            hourOffer.Project = account.CurrentProject;
            hourOffer.Name = account.Name;
            hourOffer.AccountId = account.Id;

            if (hourOffer.BeginDate.Day != hourOffer.EndDate.Day || hourOffer.BeginDate.Month != hourOffer.EndDate.Month || hourOffer.BeginDate.Year != hourOffer.EndDate.Year || hourOffer.BeginDate.Ticks >= hourOffer.EndDate.Ticks || hourOffer.BeginDate < DateTime.Now)
            {
                return BadRequest();
            }

            hourOffer.BeginDate = hourOffer.BeginDate.ToUniversalTime();
            hourOffer.EndDate = hourOffer.EndDate.ToUniversalTime();

            _repository.CreateHourOffer(hourOffer);

            _repository.SaveChanges();

            return CreatedAtAction(nameof(GetHourOfferById), new { id = hourOffer.Id }, hourOffer);

        }



        [HttpDelete("takehouroffer/{id}")]
        [Authorize]
        public ActionResult TakeHourOffer(int id)
        {
            var hourOffer = _repository.GetHourOfferById(id);
            var account = _repository.GetAccountById(this.GetUserId());

            if (hourOffer != null)
            {

                if (hourOffer.AccountId == account.Id)
                {
                    return BadRequest();
                }

                if (hourOffer.Project != account.CurrentProject)
                {
                    return Forbid();
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
                        emailsFormatted += ",";
                    }

                }

                _emailProvider.SendEmail(emailsFormatted, "HourMarket Transaction", $"{account.Name} wziął godziny {hourOffer.BeginDate} do {hourOffer.EndDate} od {hourOffer.Name}");



                _repository.DeleteHourOffer(hourOffer);
                // _repository.SaveChanges();
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

            if (hourOffer != null)
            {
                if (hourOffer.AccountId != account.Id)
                {
                    return Forbid();
                }

                _repository.DeleteHourOffer(hourOffer);
                _repository.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        [HttpPatch("{id}")]
        [Authorize]
        public ActionResult UpdateHourOffer(int id, JsonPatchDocument<HourOfferDto> patch)
        {
            var hourOffer = _repository.GetHourOfferById(id);
            var account = _repository.GetAccountById(this.GetUserId());



            if (hourOffer != null)
            {
                if (hourOffer.AccountId != account.Id)
                {
                    return Forbid();
                }


                var commandToPatch = _mapper.Map<HourOfferDto>(hourOffer);



                patch.ApplyTo(commandToPatch);

                if (!TryValidateModel(commandToPatch))
                {
                    return ValidationProblem(ModelState);
                }

                _mapper.Map(commandToPatch, hourOffer);

                hourOffer.BeginDate = hourOffer.BeginDate.ToUniversalTime();
                hourOffer.EndDate = hourOffer.EndDate.ToUniversalTime();

                _repository.SaveChanges();

                return Ok();

            }
            return NotFound();
        }

    }
}

