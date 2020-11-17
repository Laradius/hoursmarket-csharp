using HoursMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Helper
{
    public static class ProjectParser
    {

        public static int[] ParseProjects(string projects)
        {

            List<int> projectList = new List<int>();


            string[] projectString = projects.Split(";");


            foreach (string s in projectString)
            {

                projectList.Add(int.Parse(s));
            }

            return projectList.ToArray();

        }


        public static string AddProjects(params CurrentProject[] projects)
        {
            string projectString = "";

            for (int i = 0; i < projects.Length; i++)
            {



                if (i == projects.Length - 1)
                {
                    projectString += $"{(int)projects[i]}";
                }
                else
                {
                    projectString += $"{(int)projects[i]};";
                }
            }

            return projectString;
        }




    }
}
