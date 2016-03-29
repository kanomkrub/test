using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using TNI.Access;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TNI.Controllers
{
    [Route("api/[controller]")]
    public class TNIWSController : Controller
    {
        // GET api/tniws/WS_TNIREGCUST/{national_id}
        [HttpGet("WS_TNIREGCUST/{national_id}")]
        public bool WS_TNIREGCUST(string national_id)
        {
            return InformixAccess.IsCustomer(national_id);
        }

        // GET api/tniws/WS_TNIGETPOINT/{type_of_point}/{dayend_date}
        [HttpGet("WS_TNIGETPOINT/{type_of_point}/{dayend_date}")]
        public ActionResult WS_TNIGETPOINT(string type_of_point, string dayend_date)
        {
            var result = InformixAccess.GetPoint(type_of_point, dayend_date);
            return new ObjectResult(result);
        }

        // GET,POST api/tniws/WS_TNIGETPOINT + body}
        [HttpPost("WS_TNIGETPOINT")]
        [HttpGet("WS_TNIGETPOINT")]
        public ActionResult WS_TNIGETPOINT([FromBody]Dictionary<string,string> input)
        {
            var type_of_point = input.Single(t => t.Key.ToLower() == "type_of_point").Value;
            var dayend_date = input.Single(t => t.Key.ToLower() == "dayend_date").Value;
            var result = InformixAccess.GetPoint(type_of_point, dayend_date);
            return new ObjectResult(result);
        }

        [HttpGet("Test")]
        public object GetTest()
        {
            return Helper.ConnectionString;
        }
    }
}
