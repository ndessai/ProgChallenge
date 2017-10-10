using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CommonLib;
using System.Text;

namespace Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Replace")]
    public class ReplaceController : Controller
    {
        // GET: api/Replace
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Replace/<string with x 0 1>
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(string id)
        {
            var charReplace = new ReplaceChar();
            string[] result = new string[0];
            try
            {
                var items = charReplace.Compute(id);
                result = new string[items.Count];
                int i = 0;
                foreach (var item in items)
                {
                    result[i++] = new string(item);
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }
    }
}
