using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP
{
    public class HttpResponse
    {
        
        public HttpResponse(HttpResponseCode statusCode,byte [] body):
            this()
        {
            
            this.responseCode = statusCode;
            this.Body = body;
            if (body?.Length>0)
            {
                this.Headers.Add(new Header("Content-Length", Body.Length.ToString()));
            }
        }

        public  HttpResponse()
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
            var stopWatch = new Stopwatch();
            stopWatch.Start();
           
            Console.WriteLine("Watch inside httpResponse      " + stopWatch.ElapsedMilliseconds);


            var responseBuilder = new StringBuilder();
            var httpVersionAsString = this.Version switch
            {
                HttpVersionType.Http10 => "HTTP/1.0",
                HttpVersionType.Http11 => "HTTP/1.1",
                HttpVersionType.Http20 => "HTTP/2.0",
                _=>"HTTP/1.0",
            };
            responseBuilder.Append($"{httpVersionAsString} {(int)this.responseCode} {this.responseCode.ToString()}" + HtttpConstants.NewLine);
            foreach (var header in this.Headers)
            {
                responseBuilder.Append(header.ToString() + HtttpConstants.NewLine);
            }
                
            foreach (var cookie in this.Cookies)
            {
                responseBuilder.Append("Set-Cookie: " + cookie.ToString() + HtttpConstants.NewLine);   
            }
            responseBuilder.Append(HtttpConstants.NewLine);
            stopWatch.Stop();
            Console.WriteLine("Watch in the end of HttpResponse     " + stopWatch.ElapsedMilliseconds);
            return responseBuilder.ToString();
        }

    }
     

}
