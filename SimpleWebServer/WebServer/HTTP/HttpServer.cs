using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HTTP
{
    public class HttpServer : IHttpServer
    {
        private TcpListener tcpListener;


        public HttpServer(int port)
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, port);
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
                string RequestAsString = Encoding.UTF8.GetString(requestBytes,0,bytesRead);
                Console.WriteLine(RequestAsString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                throw;
            }
        }
    }
}
