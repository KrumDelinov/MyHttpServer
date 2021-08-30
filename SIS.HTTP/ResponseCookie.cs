using System;
using System.Text;

namespace SIS.HTTP
{
    public class ResponseCookie: Cookie
    {

        public ResponseCookie(string name, string value)
            :base(name, value)
        {
            this.SameSite = SameSiteType.None;
            this.Path = "/";
            this.Expires = DateTime.UtcNow.AddDays(30);
        }
        public string Domain { get; set; }

        public string Path { get; set; }

        public DateTime? Expires { get; set; }

        public long? MaxAge { get; set; }

        public bool Secured { get; set; }

        public bool HttpOnly { get; set; }

        public SameSiteType SameSite { get; set; }

        public override string ToString()
        {

            var cookieBuilder = new StringBuilder();

            cookieBuilder.Append($"{this.Name}={this.Value}");
            if (this.MaxAge.HasValue)
            {
                cookieBuilder.Append($"; Max-Age=" + this.MaxAge.Value.ToString());
            }

            else if (this.Expires.HasValue)
            {
                cookieBuilder.Append($"; Expires=" + this.Expires.Value.ToString("R"));
            }

            if (!string.IsNullOrEmpty(this.Domain))
            {
                cookieBuilder.Append($"; Domain=" + this.Domain);
            }

            if (!string.IsNullOrEmpty(this.Path))
            {
                cookieBuilder.Append($"; Path=" + this.Path);
            }

            if (this.Secured)
            {
                cookieBuilder.Append($"; Secured");
            }

            if (this.HttpOnly)
            {
                cookieBuilder.Append($"; HttpOnly");
            }
            cookieBuilder.Append($"; SameSite" + this.SameSite.ToString());

            return cookieBuilder.ToString();
        }
    }

}
