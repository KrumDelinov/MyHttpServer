using SIS.HTTP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            var routeTable = new List<Route>();
            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Get, "/users/login", LogIn));
            routeTable.Add(new Route(HttpMethodType.Post, "/users/login", DoLogIn));
            routeTable.Add(new Route(HttpMethodType.Get, "/contact", Contact));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));
            var httpServer = new HttpServer(80, routeTable);

            await httpServer.StartAsynk();
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        private static HttpResponse Contact(HttpRequest request)
        {
            string content = "<h1> Contacts </h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
            response.Headers.Add(new Header("Content-Type", "text/html"));
            return response;
        }

        public static HttpResponse Index(HttpRequest request)
        {
            string content = "<h1> Hello Header </h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
            response.Headers.Add(new Header("Content-Type", "text/html"));
            return response;
        }

        public static HttpResponse LogIn(HttpRequest request)
        {
            string content = "<h1> Login page </h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
            response.Headers.Add(new Header("Content-Type", "text/html"));
            return response;
        }

        public static HttpResponse DoLogIn(HttpRequest request)
        {
            string content = "<h1> Login page </h1>";
            byte[] stringContent = Encoding.UTF8.GetBytes(content);
            var response = new HttpResponse(HttpResponseCode.Ok, stringContent);
            response.Headers.Add(new Header("Content-Type", "text/html"));
            return response;
        }
    }
}
