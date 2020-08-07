using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Models
{
    public enum Role
    {
        Administrator = 0,
        Manager = 1,
        InnogyManager = 2,
        NFZManager = 3,
        InnogyUser = 4,
        NFZUser = 5,
        Unassigned = 6,
    }
}
