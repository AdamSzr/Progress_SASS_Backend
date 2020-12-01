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


namespace SassBackProj.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;

        private WebSocketServerConnectionManager _manager;

        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerConnectionManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context) // request 
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                string conn = _manager.AddSocket(webSocket);

                //Send ConnID Back
                await SendInfo(webSocket, conn);

                await Receive(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        Console.WriteLine($"Received->Text");
                        Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");// TODO: Change this part
                        await HandleMessage(Encoding.UTF8.GetString(buffer, 0, result.Count)); // < --- 
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        // string id = _manager.GetAllSockets().FirstOrDefault(s => s.Value == webSocket).Key; // why not conn as an id
                        // conn can will not be the same after 2 client comes
                        string id = conn;
                        Console.WriteLine($"Close on: " + id);
                        WebSocket sock;

                        if (_manager.GetAllSockets().TryRemove(id, out sock))
                            Console.WriteLine($"Removed -> From list {id}");

                        Console.WriteLine("Still Managed Connections: " + _manager.GetAllSockets().Count.ToString());

                        await sock.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Binary)
                    {
                        string id = _manager.GetAllSockets().FirstOrDefault(s => s.Value == webSocket).Key; // why not
                        Console.WriteLine("Binary Message");
                        await webSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "This server does not handle binary message type.", CancellationToken.None);
                        return;
                    }
                    else
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.ProtocolError, $"Not expected WebSocketMessageType {result.MessageType}", CancellationToken.None);
                        throw new Exception("Invalid WebSocketMessageType");
                    }
                });
            }
            else
            {
                // Console.WriteLine("Hello from 2nd Request Delegate - No WebSocket");
                await _next(context);
            }
        }

        private async Task HandleMessage(string message)
        {
            // exmaple message
            //{
            // from: Guid
            // to: guid // availeble "all"
            // message:"text"
            //}
            Models.WebSocket.MessageModel mess = JsonSerializer.Deserialize<Models.WebSocket.MessageModel>(message);

            WebSocket reciver;
            switch (mess.to)
            {
                case "all":
                    foreach (var sock in _manager.GetAllSockets())
                    {
                        if (sock.Value.State == WebSocketState.Open)
                            await sock.Value.SendAsync(Encoding.UTF8.GetBytes(mess.message), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    break;
                case "api":
                    reciver = _manager.GetAllSockets().FirstOrDefault((obj) => obj.Key == mess.from).Value;
                    string json = await new System.Net.WebClient().DownloadStringTaskAsync($"https://localhost:5001{mess.message}"); // http 
                    await reciver.SendAsync(Encoding.UTF8.GetBytes(json), WebSocketMessageType.Text, true, CancellationToken.None);
                    break;
                default:
                    reciver = _manager.GetAllSockets().FirstOrDefault((obj) => obj.Key == mess.to).Value;
                    await reciver.SendAsync(Encoding.UTF8.GetBytes(mess.message), WebSocketMessageType.Text, true, CancellationToken.None);
                    break;
            }


        }

        private async Task SendInfo(WebSocket socket, string connID)
        {
            StringBuilder str = new StringBuilder();
            _manager.GetAllSockets().All((arg) =>
            {
                str.Append(
                    arg.Key != connID ? "User ->" + arg.Key + Environment.NewLine : string.Empty
                    );
                return true;
            });

            var buffer = Encoding.UTF8.GetBytes("YourID: " + connID + Environment.NewLine + str.ToString());
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}

