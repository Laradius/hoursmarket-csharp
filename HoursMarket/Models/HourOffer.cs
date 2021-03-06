﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Models
{
    public class HourOffer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime BeginDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        [NotMapped]
        public bool Owned { get; set; }


        public int Project { get; set; }

        [JsonIgnore]
        public int AccountId { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }


    }
}
