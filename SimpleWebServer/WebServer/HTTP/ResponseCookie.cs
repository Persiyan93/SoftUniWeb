using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP
{
    public class ResponseCookie : Cookie
    {
        public ResponseCookie(string name, string value)
            : base(name, value)
        {
            this.Path = "/";
            this.SameSyte = SameSyteType.Lax;
            this.Expires = DateTime.UtcNow.AddDays(10);
        }

        public string Path { get; set; }

        public DateTime? Expires { get; set; }

        public long? MaxAge { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public SameSyteType SameSyte { get; set; }
        public string Domain { get; set; }
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
                cookieBuilder.Append($"; Expires=" + Expires.Value.ToString("R"));
            }
            if (!string.IsNullOrWhiteSpace(this.Domain))
            {
                cookieBuilder.Append($"; Domain=" + this.Domain);
            }
            if (!string.IsNullOrWhiteSpace(this.Path))
            {
                cookieBuilder.Append($"; Path=" + this.Path);
            }
            if (this.Secure)
            {
                cookieBuilder.Append("; Secure");
            }
            if (this.HttpOnly)
            {
                cookieBuilder.Append("; HttpOnly");
            }
            cookieBuilder.Append("; SameSite=" + this.SameSyte.ToString());
            return cookieBuilder.ToString();
        }
    }
}
