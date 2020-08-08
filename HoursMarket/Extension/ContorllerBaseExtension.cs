using HoursMarket.Helper;
using HoursMarket.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Extension
{
    public static class ContorllerBaseExtension
    {

        public static bool AuthorizeByRoles(this ControllerBase controller, List<Role> roles, IAuthorizer authorizer)
        {
            return authorizer.AuthorizeByRoles(roles, controller.User.Claims.FirstOrDefault(c => c.Type == "ID").Value);
        }

    }
}
