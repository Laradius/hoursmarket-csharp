using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Role { get; set; }

        [Required]
        public string CurrentProject { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(128)]
        [RegularExpression("^[A - ZŁŚa - ząęółśżźćń] +$", ErrorMessage = "Invalid characters in Name")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{8,32})", ErrorMessage = @"Password must contain minimum eight characters, at least one letter, one number and one special character")]
        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<HourOffer> HourOffers { get; set; }

    }
}
