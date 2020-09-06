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
        public int CurrentProject { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<HourOffer> HourOffers { get; set; }

    }
}
