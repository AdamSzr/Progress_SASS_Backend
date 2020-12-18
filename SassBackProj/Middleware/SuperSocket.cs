using Microsoft.AspNetCore.Http;
using SassBackProj;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading;
using SassBackProj.Models.WebSocket;

namespace SassBackProj.Middleware
{
    class SuperSocket
    {

       public event Action<WebSocket,string> OnInitialization; // send back packet of userid
        public event Action<WebSocket, MessageModel> OnTextMessage;  // returns message model
        public event Action<WebSocket,string,WebSocketReceiveResult> OnDisconnectMessage;
        public string guid { get;private set; }
        public WebSocket socket { get; private set; }
        public void Init(WebSocket socket,string guid )
        {
            this.guid = guid;
            this.socket = socket;
          //  this.OnInitialization(this.socket,this.guid);
        }


        public async Task Run()
        {

            
            var welcomeModel = Models.WebSocket.MessageModel.GetWelcomeMessage(this.guid);
            await socket.SendAsync(welcomeModel.JsonBinary(), WebSocketMessageType.Text, true, CancellationToken.None);


            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);
                
                if(result.MessageType == WebSocketMessageType.Text)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    MessageModel message = JsonSerializer.Deserialize<MessageModel>(data);

                    OnTextMessage(this.socket, message);
                }
                else if(result.MessageType == WebSocketMessageType.Close)
                {
                    this.OnDisconnectMessage(this.socket, this.guid, result);
                }
                else
                {
                    throw new Exception("Websocket Message type not implemented.");
                }

                
            }
        }
    }
}
