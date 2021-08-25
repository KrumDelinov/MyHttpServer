using System;

namespace SIS.HTTP
{
    public class HttpServerExeption : Exception
    {
        public HttpServerExeption(string message)
            :base(message)
        {
                
        }
    }
}
