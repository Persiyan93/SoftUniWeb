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
        private readonly IList<Route> routeTable;
        private readonly Dictionary<string, Dictionary<string, string>> sessions;


        public HttpServer(int port, IList<Route> routeTable)
        {
            this.routeTable = routeTable;
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
            NetworkStream networkStream = client.GetStream();
            using (networkStream)
            {
                try
                {
                    byte[] requestBytes = new byte[1000];
                    int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                    string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
                    Console.WriteLine(requestAsString);
                    var request = new HttpRequest(requestAsString);
                    Console.WriteLine(request.Cookies.Count);
                    var route = this.routeTable.Where(x => x.Path == request.Path && x.MethodType == request.MethodType).FirstOrDefault();
                    HttpResponse response;
                    if (route == null)
                    {
                        response = new HttpResponse(HttpResponseCode.NotFound, new byte[0]);
                    }
                    else
                    {
                        response = route.Action(request);
                    }


                    response.Headers.Add(new Header("Server", "Simple Server"));
                    

                    string newSessionId = null;
                    var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HtttpConstants.SessionCookieName);

                    if (sessionCookie != null && this.sessions.ContainsKey(sessionCookie.Value))
                    {
                        request.SessionData = this.sessions[sessionCookie.Value];
                    }
                    else
                    {
                        newSessionId = Guid.NewGuid().ToString();
                        var dictionary = new Dictionary<string, string>();
                        this.sessions.Add(newSessionId, dictionary);
                        request.SessionData = dictionary;
                        response.Cookies.Add(new ResponseCookie(HtttpConstants.SessionCookieName, newSessionId)
                        { HttpOnly = true });


                    }
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                    await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    await networkStream.WriteAsync(response.Body, 0, response.Body.Length);


                }


                catch (Exception e)
                {
                    Console.WriteLine("Inside catch block");
                    Console.WriteLine(e.Message);


                }
            }
        }
    }
}
