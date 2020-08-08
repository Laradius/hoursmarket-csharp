using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursMarket.Extension;
using HoursMarket.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoursMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityTestController : ControllerBase
    {
        private readonly IAuthorizer _authorizer;

        public SecurityTestController(IAuthorizer authorizer)
        {
            _authorizer = authorizer;
        }


        [Authorize]
        public IActionResult Get()
        {

            if (!this.AuthorizeByRoles(new List<Models.Role> { Models.Role.Administrator }, _authorizer))
            {
                return Forbid();
            }

            return Ok();
        }


    }
}