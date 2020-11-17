using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursMarket.Data;
using HoursMarket.Extension;
using HoursMarket.Helper;
using HoursMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoursMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminPanelController : ControllerBase
    {
        private IHoursMarketRepo _repository;
        private IAuthorizer _authorizer;

        public AdminPanelController(IHoursMarketRepo repo, IAuthorizer authorizer)
        {
            _repository = repo;
            _authorizer = authorizer;
        }

        [HttpGet]
        [Route("checkrole")]
        [Authorize]
        public ActionResult CheckRole()
        {

            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            return Ok();
        }


        [HttpGet]
        [Route("allhouroffers")]
        [Authorize]
        public ActionResult GetAllHourOffers()
        {

            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            return Ok(_repository.GetAllHourOffers());
        }

        [HttpPost]
        [Authorize]
        [Route("changerole")]
        public IActionResult ChangeRoleByEmail(AccountPermission permission)
        {
            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            Account account = _repository.GetAccountByEmail(permission.Email);

            if (account == null)
            {
                return NotFound();
            }


            try
            {
                if (Enum.IsDefined(typeof(Role), int.Parse(permission.Value)))
                {
                    account.Role = int.Parse(permission.Value);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (FormatException)
            {
                return BadRequest();
            }


            _repository.SaveChanges();
            return Ok("Role changed successfully.");
        }

        [HttpPost]
        [Authorize]
        [Route("changeproject")]
        public IActionResult ChangeProjectByEmail(AccountPermission permission)
        {
            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            Account account = _repository.GetAccountByEmail(permission.Email);

            if (account == null)
            {
                return NotFound();
            }



            try
            {
                int[] projs = ProjectParser.ParseProjects(permission.Value);

                foreach (int p in projs)
                {
                    if (!Enum.IsDefined(typeof(CurrentProject), p))
                    {
                        return BadRequest();
                    }
                }
            }
            catch (FormatException)
            {
                return BadRequest();
            }


            account.CurrentProject = permission.Value;



            _repository.SaveChanges();
            return Ok("Project changed successfully.");
        }

        [HttpGet]
        [Authorize]
        [Route("getroles")]
        public IActionResult GetAllRoles()
        {
            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            List<object> roles = new List<object>();

            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                roles.Add(new { Role = role.ToString(), Value = (int)role });
            }


            return Ok(roles);
        }





    }
}