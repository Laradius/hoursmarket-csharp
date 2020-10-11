using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursMarket.Data;
using HoursMarket.Dto;
using HoursMarket.Helper;
using HoursMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sodium;

namespace HoursMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IAuthenticator _authenticator;
        private readonly IHoursMarketRepo _repository;

        public LoginController(IAuthenticator authenticator, IHoursMarketRepo repository)
        {
            _authenticator = authenticator;
            _repository = repository;
        }


        [HttpPost]
        public IActionResult Login(AccountLoginDto account)
        {

            var acc = _repository.GetAccountByEmail(account.Email);



            if (acc == null)
            {
                return Unauthorized();
            }

            if (!PasswordHash.ScryptHashStringVerify(acc.Password, account.Password))
            {
                return Unauthorized();
            }

            Console.WriteLine($"{acc.Id} {acc.Email} {acc.Name}");

            return Ok(_authenticator.GenerateAccountAccessToken(acc));
        }

    }
}