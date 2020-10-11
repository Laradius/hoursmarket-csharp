using HoursMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Helper
{
    public interface IAuthorizer
    {
        public bool AuthorizeByRoles(List<Role> roles, string userId);
        public bool AuthorizeByCurrentProjects(List<CurrentProject> projects, string userId);

    }
}
