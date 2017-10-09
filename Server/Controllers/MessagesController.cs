using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using System.Security.Cryptography;
using System.Text;

namespace Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        private readonly ServerContext _context;

        public MessagesController(ServerContext context)
        {
            _context = context;
        }

        // GET: api/Messages
        [HttpGet]
        //eg: curl -i http://localhost:53125/api/messages
        public IEnumerable<MsgHash> GetMsgHash()
        {
            return _context.MsgHashes;
        }

        // GET: api/Messages/<digest>
        //eg: curl -i http://localhost:53125/api/messages/aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
        [HttpGet("{digest}")]
        public async Task<IActionResult> GetMsgHash([FromRoute] string digest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var msgHash = await _context.MsgHashes.SingleOrDefaultAsync(m => m.digest == digest);

            if (msgHash == null)
            {
                return NotFound(new { err_msg = "Message not found" });
            }

            return Ok(new { message = msgHash.message });
        }

        // POST: api/Messages
        //eg: curl -X POST -H "Content-Type: application/json" -d '{"message": "foo"}' http://localhost:53125/api/messages
        [HttpPost]
        public async Task<IActionResult> PostMsgHash([FromBody] MsgHash msgHash)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(msgHash.message))
            {
                return BadRequest(ModelState);
            }

            using (var hasher = SHA256.Create())
            {
                msgHash.digest = BitConverter.ToString(
                    hasher.ComputeHash(Encoding.ASCII.GetBytes(msgHash.message))).Replace("-", "");

                _context.MsgHashes.Add(msgHash);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMsgHash", new { digest = msgHash.digest }, new { digest = msgHash.digest });
            }
        }

        // DELETE: api/Messages/<digest>
        [HttpDelete("{digest}")]
        public async Task<IActionResult> DeleteMsgHash([FromRoute] string digest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var msgHash = await _context.MsgHashes.SingleOrDefaultAsync(m => m.digest == digest);
            if (msgHash == null)
            {
                return NotFound();
            }

            _context.MsgHashes.Remove(msgHash);
            await _context.SaveChangesAsync();

            return Ok(msgHash);
        }

        private bool MsgHashExists(string id)
        {
            return _context.MsgHashes.Any(e => e.digest == id);
        }
    }
}