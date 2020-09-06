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


        [HttpPost]
        [Authorize]
        [Route("changerole")]
        public IActionResult ChangeRoleById(AccountPermission permission)
        {
            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            Account account = _repository.GetAccountById(permission.Id);

            if (account == null)
            {
                return NotFound();
            }


            if (Enum.IsDefined(typeof(Role), permission.Permission))
            {
                account.Role = permission.Permission;
            }
            else
            {
                return BadRequest();
            }


            _repository.SaveChanges();
            return Ok("Role changed successfully.");
        }

        [HttpPost]
        [Authorize]
        [Route("changeproject")]
        public IActionResult ChangeProjectById(AccountPermission permission)
        {
            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            Account account = _repository.GetAccountById(permission.Id);

            if (account == null)
            {
                return NotFound();
            }


            if (Enum.IsDefined(typeof(CurrentProject), permission.Permission))
            {
                account.CurrentProject = permission.Permission;
            }
            else
            {
                return BadRequest();
            }


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

            List<string> roles = new List<string>();

            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                roles.Add($"Role: {role.ToString()}={(int)role}");
            }


            return Ok(roles);
        }

        [HttpGet]
        [Authorize]
        [Route("getcurrentprojects")]
        public IActionResult GetAllCurrentProjects()
        {
            if (!this.AuthorizeByRoles(new List<Role> { Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            List<string> projects = new List<string>();

            foreach (CurrentProject project in Enum.GetValues(typeof(CurrentProject)))
            {
                projects.Add($"Project: {project.ToString()}={(int)project}");
            }


            return Ok(projects);

        }



    }
}