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
        void CreateHourOffer(HourOffer offer);

        bool SaveChanges();
        void DeleteHourOffer(HourOffer offer);

        Account GetAccountById(int id);
        Account GetAccountByEmail(string email);

        void DeleteAccount(int id);

        void CreateAccount(Account account);

        IEnumerable<ManagerEmail> GetManagerEmailsByProject(CurrentProject project);

    }
}
