using AutoMapper;
using HoursMarket.Dto;
using HoursMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Profiles
{
    public class HourOfferProfile : Profile
    {

        public HourOfferProfile()
        {
            CreateMap<HourOfferDto, HourOffer>();
            CreateMap<HourOffer, HourOfferDto>();
        }
    }
}
