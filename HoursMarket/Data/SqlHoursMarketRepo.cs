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

        public void CreateAccount(Account account)
        {
            _context.Accounts.Add(account);
        }

        public void CreateHourOffer(HourOffer offer)
        {
            _context.HourOffers.Add(offer);
        }

        public void DeleteAccount(int id)
        {
            _context.Remove(GetAccountById(id));
        }

        public void DeleteHourOffer(HourOffer offer)
        {
            _context.HourOffers.Remove(offer);
        }

        public Account GetAccountByEmail(string email)


        {
            return _context.Accounts.FirstOrDefault(x => email == x.Email);
        }

        public Account GetAccountById(int id)
        {
            return _context.Accounts.FirstOrDefault(x => id == x.Id);
        }

        public IEnumerable<HourOffer> GetAllHourOffers()
        {

            return SortAndDeleteHourOffers();
        }

        public HourOffer GetHourOfferById(int id)
        {
            return _context.HourOffers.FirstOrDefault(h => id == h.Id);
        }

        public IEnumerable<ManagerEmail> GetManagerEmailsByProject(CurrentProject project)
        {
            return _context.ManagerEmails.Where(x => x.ProjectId == (int)project).ToList();
        }

        private IEnumerable<HourOffer> SortAndDeleteHourOffers()
        {
            var list = _context.HourOffers.ToList();
            list.Sort((x, y) => DateTime.Compare(x.BeginDate, y.BeginDate));
            list.RemoveAll(x => x.BeginDate < DateTime.Now);
            return list;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
