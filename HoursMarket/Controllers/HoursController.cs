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
        [Route("allhouroffers")]
        [Authorize]
        public ActionResult GetAllHourOffers()
        {

            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            return Ok(_repository.GetAllHourOffers());
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetAuthorizedHourOffers()
        {
            var account = _repository.GetAccountById(this.GetUserId());
            return Ok(_repository.GetAllHourOffers().Where(x => x.AccountId == account.Id));
        }




        [HttpGet("{id}")]
        public ActionResult GetHourOfferById(int id)
        {
            var hourOffer = _repository.GetHourOfferById(id);
            if (hourOffer != null)
            {
                return Ok(hourOffer);
            }
            return NotFound();

        }

        [HttpPost]
        public ActionResult CreateHourOffer(HourOfferDto offer)
        {

            var hourOffer = _mapper.Map<HourOffer>(offer);

            _repository.CreateHourOffer(hourOffer);
            _repository.SaveChanges();

            return CreatedAtAction(nameof(GetHourOfferById), new { id = hourOffer.Id }, hourOffer);

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteHourOffer(int id)
        {
            var hourOffer = _repository.GetHourOfferById(id);

            if (hourOffer != null)
            {
                _repository.DeleteHourOffer(hourOffer);
                _repository.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        [HttpPatch("{id}")]
        public ActionResult UpdateHourOffer(int id, JsonPatchDocument<HourOfferDto> patch)
        {
            var hourOffer = _repository.GetHourOfferById(id);

            if (hourOffer != null)
            {
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

