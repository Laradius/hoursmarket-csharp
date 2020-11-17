using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Dto
{
    public class AccountPasswordDto
    {

        [Required]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{8,32})", ErrorMessage = @"Password must contain minimum eight characters, at least one letter, one number and one special character")]
        public string Password { get; set; }

    }
}
