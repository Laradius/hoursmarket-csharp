using HoursMarket.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Data
{
    public class HoursMarketContext : DbContext
    {


        public HoursMarketContext(DbContextOptions<HoursMarketContext> opt) : base(opt)
        {

        }

        public DbSet<HourOffer> HourOffers { get; set; }
        public DbSet<Account> Accounts { get; set; }

    }
}
