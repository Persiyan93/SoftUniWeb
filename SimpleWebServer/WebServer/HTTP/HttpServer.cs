using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTP
{
    public class HttpServer : IHttpServer
    {
        private Object lockObject;
        private TcpListener tcpListener;
        private readonly IList<Route> routeTable;
        private readonly Dictionary<string, Dictionary<string, string>> sessions;


        public HttpServer(int port, IList<Route> routeTable)
        {
            lockObject = new Object();
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
                await Task.Run(() => ProcessClientAsync(client));

            }
        }




        private async void ProcessClientAsync(TcpClient client)
        {

            NetworkStream networkStream = client.GetStream();

            using (networkStream)
            {
                try
                {
                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    byte[] requestBytes = new byte[4096];
                    string requestAsString = null;
                    using MemoryStream memoryStream = new MemoryStream();
                    while (true)
                    {
                        int bytesRead = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                          await memoryStream.WriteAsync(requestBytes, 0, bytesRead);
                        if (bytesRead<requestBytes.Length)
                        {
                            break;
                        }


                    }

                    Console.WriteLine("Finish with Reading " + stopWatch.ElapsedMilliseconds);

                    requestAsString = Encoding.UTF8.GetString(memoryStream.ToArray());


                    Console.WriteLine("Before Encoding   " + stopWatch.ElapsedMilliseconds);
                    Console.WriteLine(requestAsString);
                    Console.WriteLine("Before parceing Request    " + stopWatch.ElapsedMilliseconds);
                    var request = new HttpRequest(requestAsString);
                    Console.WriteLine("Aftere Parceing of Request    " + stopWatch.ElapsedMilliseconds);

                    var route = this.routeTable.Where(x => x.Path == request.Path && x.MethodType == request.MethodType).FirstOrDefault();
                    Console.WriteLine(request.QueryData);
                    HttpResponse response;

                    if (route == null)
                    {

                        Console.WriteLine(request.MethodType);
                        Console.WriteLine(requestAsString);
                        Console.WriteLine("Finish with This Cient  " + stopWatch.ElapsedMilliseconds);
                        return;

                        // response = new HttpResponse(HttpResponseCode.NotFound, new byte[0]);

                    }

                    else
                    {
                        Console.WriteLine("Before Action      " + stopWatch.ElapsedMilliseconds);
                        response = route.Action(request);
                        Console.WriteLine("After action      " + stopWatch.ElapsedMilliseconds);
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

                    Console.WriteLine(stopWatch.ElapsedMilliseconds);
                    await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine(stopWatch.ElapsedMilliseconds);
                    await networkStream.WriteAsync(response.Body, responseBytes.Length, response.Body.Length);

                    stopWatch.Stop();
                    Console.WriteLine("Finish with This Cient  " + stopWatch.ElapsedMilliseconds);


                }


                catch (Exception e)
                {
                    Console.WriteLine("Inside catch block");
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.Message);


                }
            }
        }
    }
}
