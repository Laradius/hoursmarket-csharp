using HoursMarket.Dto;
using HoursMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Helper
{
    public interface IAuthenticator
    {

        public string GenerateAccountAccessToken(Account account);
        public string GenerateRegistrationToken(AccountRegistrationDto accountRegistration);


    }
}
