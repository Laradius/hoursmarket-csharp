using HoursMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Data
{
    public interface IHoursMarketRepo
    {

        IEnumerable<HourOffer> GetAllHourOffers();
        HourOffer GetHourOfferById(int id);



    }
}
