using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet("/tokenTest")]
        public IActionResult TokenTest()
        {
            return Ok("Authorized");
        }

        [HttpGet("/tokenTest2")]
        public IActionResult TokenTest2()
        {
            return Ok("Authorized");
        }
    }
}
