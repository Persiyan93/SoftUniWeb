using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP
{
    public class HttpResponse
    {
        public HttpResponse(HttpResponseCode code,byte [] body):
            this()
        {
            this.responseCode = code;
            this.Body = body;
            if (body?.Length>0)
            {
                this.Headers.Add(new Header("Content-Length", Body.Length.ToString()));
            }
        }

        public HttpResponse()
        {
            this.Version = HttpVersionType.Http10;
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();
        }
        public HttpVersionType Version { get; set; }

        public IList<Header> Headers { get; set; }

        public IList<Cookie> Cookies { get; set; }

        public HttpResponseCode responseCode { get; set; }
        public byte[] Body { get; set; }
        public override string ToString()
        {

            var responseBuilder = new StringBuilder();
            var httpVersionAsString = this.Version switch
            {
                HttpVersionType.Http10 => "HTTP/1.0",
                HttpVersionType.Http11 => "HTTP/1.1",
                HttpVersionType.Http20 => "HTTP/2.0",
            };
            responseBuilder.Append($"{httpVersionAsString} {(int)responseCode} {responseCode.ToString()}" + HtttpConstants.NewLine);
            foreach (var header in this.Headers)
            {
                responseBuilder.Append(header.ToString() + HtttpConstants.NewLine);
            }
                
            foreach (var cookie in this.Cookies)
            {
                responseBuilder.Append("Set-Cookie: " + cookie.ToString() + HtttpConstants.NewLine);   
            }
            responseBuilder.Append(HtttpConstants.NewLine);
            return responseBuilder.ToString();
        }

    }


}
