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

        public HoursController(IHoursMarketRepo repo, IMapper mapper, IAuthorizer authorizer)
        {
            _repository = repo;
            _mapper = mapper;
            _authorizer = authorizer;
        }




        [HttpGet]
        [Authorize]
        public ActionResult GetAuthorizedHourOffers()
        {
            var account = _repository.GetAccountById(this.GetUserId());
            return Ok(_repository.GetAllHourOffers().Where(x => x.Project == account.CurrentProject));
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

            _repository.CreateHourOffer(hourOffer);
            _repository.SaveChanges();

            return CreatedAtAction(nameof(GetHourOfferById), new { id = hourOffer.Id }, hourOffer);

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
                _repository.SaveChanges();

                return Ok();

            }
            return NotFound();
        }

    }
}

