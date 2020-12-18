using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SassBackProj.Models.WebSocket
{
    public class MessageModel
    {
        public string action { get; set; } // TODO: WSAction.
        public string data { get; set; }
        public string JsonSerialize() => System.Text.Json.JsonSerializer.Serialize(this, typeof(MessageModel));
        public byte[] JsonBinary() => System.Text.ASCIIEncoding.ASCII.GetBytes(this.JsonSerialize());
        public static MessageModel GetWelcomeMessage(string id)
        {
            SassBackProj.Models.WebSocket.MessageModel idPacket = new Models.WebSocket.MessageModel();

            idPacket.action = "welcome";
            idPacket.data = string.Format("{id:\"{0}\"}", id);
            return idPacket;
            }
    }
}



public enum WSAction
{
    message, // socket, messagemodel 
    disconnected, // przekazuje socket.
}
