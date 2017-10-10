using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CommonLib;
using System.IO;
using System.Text;

namespace Server.Controllers
{
    public class CatalogData
    {
        public string catalog { get; set; }
        public uint friends { get; set; }
        public uint limit { get; set; }
    }
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
        public async Task<IActionResult> Post([FromBody]CatalogData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lines = new List<string>();
            using (var reader = new StringReader(data.catalog))
            {
                var line = reader.ReadLine();
                while (!string.IsNullOrWhiteSpace(line))
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }
            var optimalPairs = new OptimalPairs();
            try
            {
                var items = optimalPairs.Compute(lines, data.limit, data.friends);
                var sb = new StringBuilder();
                foreach (var item in items)
                {
                    sb.Append(item.Name + " " + item.Price + "     ");
                }
                if (items.Count <= 0)
                {
                    sb.Append("Not Possible");
                }
                return Ok(sb.ToString());
            }
            catch (ApplicationException ex)
            {
                return Ok(ex.Message);
            }
        }
        
    }
}
