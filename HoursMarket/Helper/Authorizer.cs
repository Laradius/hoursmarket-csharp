﻿using System;
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



        public bool AuthorizeByCurrentProjects(List<CurrentProject> projects, string userId)
        {

            var user = _repository.GetAccountById(int.Parse(userId));

            if (user == null)
            {
                return false;
            }

            foreach (CurrentProject p in projects)
            {

                int[] currentProjects = ProjectParser.ParseProjects(user.CurrentProject);

                foreach (int proj in currentProjects)
                {
                    if ((int)p == proj)
                    {
                        return true;
                    }
                }


            }
            return false;

        }

        public bool AuthorizeByRoles(List<Role> roles, string userId)
        {

            var user = _repository.GetAccountById(int.Parse(userId));

            if (user == null)
            {
                return false;
            }

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
