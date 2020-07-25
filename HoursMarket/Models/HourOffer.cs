using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Models
{
    public class HourOffer
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }


    }
}
