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
using Microsoft.EntityFrameworkCore;
using Sodium;

namespace HoursMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IAuthenticator _authenticator;
        private readonly IHoursMarketRepo _repository;
        private readonly IEmailSender _email;



        private bool CanRegister(string email)
        {
            if (_repository.GetAccountByEmail(email) != null)
            {
                return false;
            }
            return true;
        }

        public RegisterController(IAuthenticator authenticator, IHoursMarketRepo repository, IEmailSender email)
        {
            _authenticator = authenticator;
            _repository = repository;
            _email = email;
        }


        [HttpPost]

        [AllowAnonymous]
        public IActionResult GenerateRegistrationToken(AccountRegistrationDto account)
        {
            // return Ok(_authenticator.GenerateRegistrationToken(account));

            if (!CanRegister(account.Email))
            {
                return Conflict("An account is already registered on that email.");
            }

            _email.SendEmail(account.Email, "Account Registration Link", @"http://hourmarket.hostingasp.pl/#/registerend" + @"/?token=" + _authenticator.GenerateRegistrationToken(account));
            return Ok("Activation link sent");


            // return Ok(HttpContext.Request.Host.Value + "/?token=" + _authenticator.GenerateRegistrationToken(account));
        }




        [Route("finish")]
        [Authorize]
        [HttpPost]
        public IActionResult EndRegistration(AccountPasswordDto password)
        {



            var name = User.Claims.FirstOrDefault(c => c.Type == "name").Value;
            var email = User.Claims.FirstOrDefault(c => c.Type == "e-mail").Value;

            if (!CanRegister(email))
            {
                return Conflict("Cannot finish registratoin. An account is already registered on that email.");
            }

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(email))
            {
                return BadRequest();
            }

            Account acc = new Account();
            acc.Email = email;
            acc.Name = name;


            acc.Password = PasswordHash.ScryptHashString(password.Password, PasswordHash.Strength.Medium);
            acc.Role = (int)Role.User;
            acc.CurrentProject = (int)CurrentProject.Unassigned;

            try
            {
                _repository.CreateAccount(acc);
                _repository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }


            return Ok("Account created successfully.");
        }

    }
}