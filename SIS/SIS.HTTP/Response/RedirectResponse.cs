using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Response
{
    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string newLocation)
        {
            this.Headers.Add(new Header("Location", newLocation));
            this.StatusCode = HttpResponseCode.Found;
        }
    }
}
