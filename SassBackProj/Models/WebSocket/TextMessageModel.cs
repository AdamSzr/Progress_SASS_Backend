using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SassBackProj.Models.WebSocket
{
    public class TextMessageModel
    {
        public string from { get; set; }
        public string to { get; set; }
        public string message { get; set; }

        public string JsonSerialize() => System.Text.Json.JsonSerializer.Serialize(this, typeof(Models.WebSocket.TextMessageModel));
        public byte[] JsonBinary() => System.Text.ASCIIEncoding.ASCII.GetBytes(this.JsonSerialize());
    }
}
