using SIS.HTTP;
using SIS.HTTP.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    public static class Program
    {
        //Test for GitHub
        public static async Task  Main(string[] args)
        {
            var db = new AplicatinDbContext();
            db.Database.EnsureCreated();

            var routeTable = new List<Route>();
            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Post, "/Tweets/Create", CreateTweet));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));
            var httpServer = new HttpServer(80, routeTable);

            await httpServer.StartAsynk();
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {
            var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
            return new FileResponse(byteContent, "image/x-icon");
        }


        public static HttpResponse Index(HttpRequest request)
        {
            return new HtmlResponse("<form action='/Tweets/Create' method='post'><input name='creator' /><br /><textarea name='tweetName'></textarea><br /><input type='submit' /></form>");
        }

        public static HtmlResponse CreateTweet(HttpRequest request)
        {
            var db = new AplicatinDbContext();
            db.Add(new Tweet
            {
                CreatOn = DateTime.UtcNow,
                Creator = request.FormData["creator"],
                Content = request.FormData["tweetName"],
            });

            db.SaveChanges();
            return new HtmlResponse("Ready");
        }



    }
}
