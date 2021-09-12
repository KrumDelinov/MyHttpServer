using SIS.HTTP;
using SIS.HTTP.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    class Program
    {
        //Test for GitHub
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
            var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
            return new FileResponse(byteContent, "image/x-icon");
        }

        private static HttpResponse Contact(HttpRequest request)
        {
            return new HtmlResponse("<h1> Contacts </h1>");
        }

        public static HttpResponse Index(HttpRequest request)
        {
            return new HtmlResponse("<h1> Hello Header </h1>");
        }

        public static HttpResponse LogIn(HttpRequest request)
        {
            return new HtmlResponse("<h1> Login page </h1>");
        }

        public static HttpResponse DoLogIn(HttpRequest request)
        {
            return new HtmlResponse("<h1> Login page </h1>");
           
        }
    }
}
