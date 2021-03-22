using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HTTP
{
    public class HttpServer : IHttpServer
    {
        private TcpListener tcpListener;
        //private readonly IList<Route> routeTable;
        private readonly Dictionary<string, Dictionary<string, string>> sessions;


        public HttpServer(int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, port);
            this.sessions = new Dictionary<string, Dictionary<string, string>>();
        }
        public async Task ResetAsync()
        {
            this.Stop();
            await this.StratAsync();
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }

        public async Task StratAsync()
        {
            this.tcpListener.Start();
            while (true)
            {
                TcpClient client = await tcpListener.AcceptTcpClientAsync();
                Task.Run(() => ProcessClientAsync(client));

            }
        }
        private async void ProcessClientAsync(TcpClient client)
        {
            using NetworkStream networkStream = client.GetStream();
            try
            {
                byte[] requestBytes = new byte [1000];
                int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                string requestAsString = Encoding.UTF8.GetString(requestBytes,0,bytesRead);
                Console.WriteLine(requestAsString);
                var request = new HttpRequest(requestAsString);
                Console.WriteLine(request.Cookies.Count);
                string content = "<h1>random page</h1>";
                
                byte[] stringContent = Encoding.UTF8.GetBytes(content);
                var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
                response.Headers.Add(new Header("Server", "Simple Server"));
                response.Headers.Add(new Header("Content-Type", "text/html"));
                response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString())
                { HttpOnly = true, MaxAge = 3600 });
                byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(response.Body, 0, response.Body.Length);




             
             








                //string newSessionId = null;
                //var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == "SID");
                //if (sessionCookie != null||this.sessions.ContainsKey(sessionCookie.Value))
                //{
                //    request.SessionData = this.sessions[sessionCookie.Value];
                //}
                //else 
                //{
                //    newSessionId = Guid.NewGuid().ToString();
                //    var dictionary = new Dictionary<string, string>();
                //    this.sessions.Add(newSessionId, dictionary);
                //    request.SessionData = dictionary;


                //}
            }
            catch (HttpException e)
            {
                Console.WriteLine(e.Message);

                throw;
            }
        }
    }
}
