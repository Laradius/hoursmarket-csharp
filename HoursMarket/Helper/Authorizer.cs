using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursMarket.Data;
using HoursMarket.Models;

namespace HoursMarket.Helper
{
    public class Authorizer : IAuthorizer
    {
        private readonly IHoursMarketRepo _repository;

        public Authorizer(IHoursMarketRepo repository)
        {

            _repository = repository;
        }


        public bool AuthorizeByRoles(List<Role> roles, string userId)
        {

            var user = _repository.GetAccountById(int.Parse(userId));

            foreach (Role r in roles)
            {
                if ((int)r == user.Role)
                {
                    return true;
                }
            }
            return false;

        }
    }
}
