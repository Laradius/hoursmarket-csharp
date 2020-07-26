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

        public void CreateHourOffer(HourOffer offer)
        {
            _context.HourOffers.Add(offer);
        }

        public void DeleteHourOffer(HourOffer offer)
        {
            _context.HourOffers.Remove(offer);
        }

        public IEnumerable<HourOffer> GetAllHourOffers()
        {
            return _context.HourOffers.ToList();
        }

        public HourOffer GetHourOfferById(int id)
        {
            return _context.HourOffers.FirstOrDefault(h => id == h.Id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
