
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class HttpRequest
    {

        public HttpRequest(string HttpRequestAsString)
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();

            var lines = HttpRequestAsString.Split(new string[] { HttpConstants.NewLine }, StringSplitOptions.None);

            var httpInfoHeader = lines[0];
            var infoHeaderParts = httpInfoHeader.Split(' ');
            var httpMethod = infoHeaderParts[0];

            if (infoHeaderParts.Length != 3)
            {
                throw new HttpServerExeption("Invalid HTTP header line");
            }

            this.Method = httpMethod switch
            {
                "POST" => HttpMethodType.Post,
                "GET" => HttpMethodType.Get,
                "PUT" => HttpMethodType.Put,
                "DELETE" => HttpMethodType.Delete,
                _ => HttpMethodType.Unknown

            };

            this.Path = infoHeaderParts[1];

            var httpVersion = infoHeaderParts[2];

            this.Version = httpVersion switch
            {
                "HTTP/1.0" => HttpVersionType.Http10,
                "HTTP/1.1" => HttpVersionType.Http11,
                "HTTP/2.0" => HttpVersionType.Http20,
                _ => HttpVersionType.Http11,
            };

            bool isHeader = true;
            var bodyBuilder = new StringBuilder();

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    isHeader = false;
                    continue;
                }

                if (isHeader)
                {
                    var headerParts = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);

                    if (headerParts.Length > 2)
                    {
                        throw new HttpServerExeption($"Invalid headet {line}");
                    }

                    var header = new Header(headerParts[0], headerParts[1]);

                    this.Headers.Add(header);

                    if (headerParts[0] == "Cookie")
                    {
                        var cookiesAsString = headerParts[1];
                        var cookies = cookiesAsString.Split(new string[] { " ;" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var cookieAsString in cookies)
                        {
                            var cookieParts = cookieAsString.Split(new char[] { '='}, 2);
                            if (cookieParts.Length == 2)
                            {
                                this.Cookies.Add(new Cookie(cookieParts[0], cookieParts[1]));
                            }
                        }
                    }
                }
                else
                {
                    bodyBuilder.Append(line);
                }
            }

        }
        public HttpMethodType Method { get; set; }

        public string Path { get; set; }

        public HttpVersionType Version { get; set; }

        public IList<Header> Headers { get; set; }

        public IList<Cookie> Cookies { get; set; }

        public string Body { get; set; }
    }
}
