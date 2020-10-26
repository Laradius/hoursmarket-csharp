using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Models
{
    public class AccountPermission
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public int Value { get; set; }

    }
}
