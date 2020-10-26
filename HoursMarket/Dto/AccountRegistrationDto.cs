using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Dto
{
    public class AccountRegistrationDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        public string Name { get; set; }

    }
}
