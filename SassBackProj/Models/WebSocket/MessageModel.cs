using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SassBackProj.Models.WebSocket
{
    public class MessageModel
    {
        public string from { get; set; }
        public string to { get; set; }
        public string message { get; set; }
    }
}
