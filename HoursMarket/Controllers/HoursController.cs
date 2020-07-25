using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursMarket.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HoursMarket.Controllers
{
    [ApiController]
    [Route("api/houroffers")]
    public class HoursController : ControllerBase
    {

        readonly IHoursMarketRepo _repository;

        public HoursController(IHoursMarketRepo repo)
        {
            _repository = repo;
        }

        [HttpGet]
        public ActionResult GetAllHourOffers()
        {
            return Ok(_repository.GetAllHourOffers());
        }


        [HttpGet("{id}")]
        public ActionResult GetAllHourOffers(int id)
        {
            var hourOffer = _repository.GetHourOfferById(id);
            if (hourOffer != null)
            {
                return Ok(hourOffer);
            }
            return NotFound();

        }

    }
}

