using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursMarket.Models;

namespace HoursMarket.Data
{
    public class SqlHoursMarketRepo : IHoursMarketRepo
    {
        private readonly HoursMarketContext _context;

        public SqlHoursMarketRepo(HoursMarketContext context)
        {
            _context = context;
        }

        public IEnumerable<HourOffer> GetAllHourOffers()
        {
            return _context.HourOffers.ToList();
        }

        public HourOffer GetHourOfferById(int id)
        {
            return _context.HourOffers.FirstOrDefault(h => id == h.Id);
        }
    }
}
