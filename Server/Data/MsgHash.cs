using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Data
{
    public class MsgHash
    {
        [Key]
        public string digest { get; set; }
        public string message { get; set; }
    }
}
