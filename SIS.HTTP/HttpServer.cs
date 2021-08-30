using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIS.HTTP
{
    public class HttpServer : IHttpServer
    {
        private readonly TcpListener tcpListener;

        public HttpServer(int port)
        {
             this.tcpListener = new TcpListener(IPAddress.Loopback, port);
            

        }
        public async Task ResetAsynk()
        {
            this.Stop();
            await this.StartAsynk();
        }

        public async Task StartAsynk()
        {
            this.tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () => { await ProcessClientAsynk(tcpClient); });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }

        private async Task ProcessClientAsynk(TcpClient tcpClient)
        {


            using (NetworkStream networkSream = tcpClient.GetStream())
            {
                byte[] requestBytes = new byte[1000000];
                int bytesRead = await networkSream.ReadAsync(requestBytes, 0, requestBytes.Length);
                string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
                var request = new HttpRequest(requestAsString);
                string content = "<h1> Hello Header </h1>";

                if (request.Path == "/home")
                {
                    content = "<h1> Home Page </h1>";
                }
                byte [] stringContent = Encoding.UTF8.GetBytes(content);
                var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
                response.Headers.Add(new Header ("Server", "KrumServer 1.0"));
                response.Headers.Add(new Header("Content-Type", "text/html"));
                response.Cookies.Add(
                    new ResponseCookie("sid", Guid.NewGuid().ToString())
                    {HttpOnly = true, MaxAge = 3600, });

                byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());

                await networkSream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkSream.WriteAsync(response.Body, 0, response.Body.Length);

                Console.WriteLine(request);

                Console.WriteLine(new string('=', 60));
            };
        }
    }
}
