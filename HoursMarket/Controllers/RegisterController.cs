using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HoursMarket.Data;
using HoursMarket.Dto;
using HoursMarket.Helper;
using HoursMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoursMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IAuthenticator _authenticator;
        private readonly IHoursMarketRepo _repository;

        public RegisterController(IAuthenticator authenticator, IHoursMarketRepo repository)
        {
            _authenticator = authenticator;
            _repository = repository;
        }


        [HttpPost]
        [Route("begin")]
        [AllowAnonymous]
        public IActionResult GenerateRegistrationToken(AccountRegistrationDto account)
        {
            return Ok(_authenticator.GenerateRegistrationToken(account));
        }

        [Route("finish")]
        [Authorize]
        public IActionResult EndRegistration(AccountPasswordDto password)
        {


            var name = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == "e-mail").Value;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(email))
            {
                return BadRequest();
            }

            Account acc = new Account();
            acc.Email = email;
            acc.Name = name;
            acc.Password = password.Password;
            acc.Role = (int)Role.Unassigned;


            _repository.CreateAccount(acc);
            _repository.SaveChanges();


            return Ok(acc);
        }

    }
}