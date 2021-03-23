using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.Response
{
    public class HtmlResponse:HttpResponse
    {
        public HtmlResponse(string html)
        {
            
            this.responseCode = HttpResponseCode.Ok;
            this.Body = Encoding.UTF8.GetBytes(html);
            this.Headers.Add(new Header("Content-Type", "text/html"));
            this.Headers.Add(new Header("Content-Length ", this.Body.Length.ToString()));
        }
    }
}
