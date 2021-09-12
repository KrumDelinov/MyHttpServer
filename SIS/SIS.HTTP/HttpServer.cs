using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IList<Route> routeTable;
        private readonly IDictionary<string, IDictionary<string, string>> session;

        public HttpServer(int port, IList<Route> routeTable)
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, port);
            this.routeTable = routeTable;
            this.session = new Dictionary<string, IDictionary<string, string>>();
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
            using NetworkStream networkSream = tcpClient.GetStream();
            try
            {
                byte[] requestBytes = new byte[1000000];
                int bytesRead = await networkSream.ReadAsync(requestBytes, 0, requestBytes.Length);
                string requestAsString = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
                var request = new HttpRequest(requestAsString);

                var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionIdCookieName);
                string newSesionId = null;
                if (sessionCookie != null && this.session.ContainsKey(sessionCookie.Value))
                {
                    request.SessionData = this.session[sessionCookie.Value];
                }

                else
                {
                    var dictionary = new Dictionary<string, string>();
                    newSesionId = Guid.NewGuid().ToString();
                    this.session.Add(newSesionId, dictionary);
                    request.SessionData = dictionary;
                }

                Console.WriteLine($"{request.Method} {request.Path}");
                var route = this.routeTable.FirstOrDefault(x => x.HttpMethod == request.Method && x.Path == request.Path);
                HttpResponse response;

                if (route == null)
                {
                    response = new HttpResponse(HttpResponseCode.NotFound, new byte[0]);
                }

                else
                {
                    response = route.Action(request);
                }
                response.Headers.Add(new Header("Server", "KrumServer 1.0"));

                
                if (new)
                {
                    
                    response.Cookies.Add(
                    new ResponseCookie(HttpConstants.SessionIdCookieName, newSesionId)
                    { HttpOnly = true, MaxAge = 3600 * 30, });
                }
             

                byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());

                await networkSream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkSream.WriteAsync(response.Body, 0, response.Body.Length);

            }
            catch (Exception ex)
            {

                var errorResponse = new HttpResponse(HttpResponseCode.InernalServerError, Encoding.UTF8.GetBytes(ex.ToString()));
                errorResponse.Headers.Add(new Header("Content-Type", "text/plain"));
                byte[] responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());
                await networkSream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkSream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);
            }
      
        }
    }
}
