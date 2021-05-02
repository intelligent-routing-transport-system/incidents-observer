using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace incidents_observer.Controllers
{
    [Route("api/healthcheck")]
    public class HealthcheckController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("HealthCheck: " + DateTime.Now.ToString());
        }
    }
}
