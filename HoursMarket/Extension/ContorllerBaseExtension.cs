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

        public static bool AuthorizeByCurrentProjects(this ControllerBase controller, List<CurrentProject> projects, IAuthorizer authorizer)
        {
            return authorizer.AuthorizeByCurrentProjects(projects, controller.User.Claims.FirstOrDefault(c => c.Type == "ID").Value);
        }


        public static int GetUserId(this ControllerBase controller)
        {
            return int.Parse(controller.User.Claims.FirstOrDefault(c => c.Type == "ID").Value);
        }

    }
}
