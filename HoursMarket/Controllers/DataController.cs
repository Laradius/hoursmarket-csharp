using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoursMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoursMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        [Route("getcurrentprojects")]
        public IActionResult GetAllCurrentProjects()
        {
        
            List<object> projects = new List<object>();

            foreach (CurrentProject project in Enum.GetValues(typeof(CurrentProject)))
            {
                projects.Add(new { Project = project.ToString(), Value = (int)project });
            }


            return Ok(projects);

        }

    }
}