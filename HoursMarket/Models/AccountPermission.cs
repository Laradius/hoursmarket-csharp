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
        public int Id { get; set; }
        public string Email { get; set; }

        public int Permission { get; set; }

    }
}
