using SIS.HTTP;
using SIS.HTTP.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var db = new AplicatinDbContext();
            var tweets = db.Tweets.Select(x => new
            {
                x.CreatOn,
                x.Creator,
                x.Content,
            }).ToList();

            StringBuilder html = new StringBuilder();
            html.Append("<table><tr><th>Data</th><th>Name</th><th>Tweet</th></tr>");

            foreach (var item in tweets)
            {
                html.Append($"<tr><td>{item.CreatOn}</td><td>{item.Creator}</td><td>{item.Content}</td></tr>");
            }
            html.Append("</table>");
            html.Append("<form action='/Tweets/Create' method='post'><input name='creator' /><br /><textarea name='tweetName'></textarea><br /><input type='submit' /></form>");
            return new HtmlResponse(html.ToString());


        }

        public static HttpResponse CreateTweet(HttpRequest request)
        {
            var db = new AplicatinDbContext();
            db.Add(new Tweet
            {
                CreatOn = DateTime.UtcNow,
                Creator = request.FormData["creator"],
                Content = request.FormData["tweetName"],
            });

            db.SaveChanges();
            return new RedirectResponse("/");
        }



    }
}
