using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Produces("application/json")]
    [Route("api/FindPairs")]
    public class FindPairsController : Controller
    {
        // GET: api/FindPairs
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/FindPairs
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
    }
}
