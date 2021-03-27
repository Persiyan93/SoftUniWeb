using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace HTTP
{
    public  class HttpRequest
    {

        private string requestAsString;
        public HttpRequest(string requestAsString)
        {
           
           
            this.Cookies = new List<Cookie>();
            this.Headers = new List<Header>();
            this.SessionData = new Dictionary<string, string>();
            this.FormData = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(requestAsString)) 
            {
                return;
            }
            
            this.requestAsString = requestAsString;
            var resultLines = requestAsString.Split(new[] { HtttpConstants.NewLine }, StringSplitOptions.None);
            var httpinfoHeader = resultLines[0];
            var infoheaderParts = httpinfoHeader.Split(" ");
            if (infoheaderParts.Length!=3)
            {
                throw new HttpException("Invalid Http headr line");

            }
           

            // Set Method Type

            var httpMethod = infoheaderParts[0];
            this.MethodType = httpMethod switch
            {
                "GET" => HttpMethodType.Get,
                "POST" => HttpMethodType.Post,
                "PUT" => HttpMethodType.Put,
                "DELETE" => HttpMethodType.Delete,
                _ => HttpMethodType.Unknown

            };
            

            // Set Path
            this.Path = infoheaderParts[1];
            if (this.Path.Contains("?"))
            {
                var parts = this.Path.Split("?", 2);
                var queryString =parts[1] ;
                this.Path =parts[0];
                this.QueryData = new Dictionary<string, string>();
                DataParser(QueryData, queryString);

            }

          
            //Set Http Version
            var httpVersion = infoheaderParts[2];
             this.Version = httpVersion switch
            {
                "HTTP/1.0" => HttpVersionType.Http10,
                "HTTP/1.1" => HttpVersionType.Http11,
                "HTTP/2.0" => HttpVersionType.Http20,
                _ => HttpVersionType.Http11
            };
         
            var bodyBuilder = new StringBuilder();
            bool isInHeader = true;
            for (int i = 1 ; i < resultLines.Length; i++)
            {
                var currLine = resultLines[i];
                if (string.IsNullOrEmpty(currLine))
                {
                    isInHeader = false;
                    continue;
                }
                if (isInHeader)
                {
                    var headerParts = currLine.Split(": ", 2, StringSplitOptions.None);
                    if (headerParts.Length!=2)
                    {
                        throw new HttpException($"Invalid header {currLine}");
                    }
                   this.Headers.Add(new Header(headerParts[0], headerParts[1]));
                    if (headerParts[0]=="Cookie")
                    {
                        var cookieAsString = headerParts[1];
                        var cookies = cookieAsString.Split("; ", StringSplitOptions.RemoveEmptyEntries);
                        foreach (var cookie in cookies)
                        {
                            var cookieParts = cookie.Split("=", 2, StringSplitOptions.None);
                            if (cookieParts.Length==2)
                            {
                                this.Cookies.Add(new Cookie(cookieParts[0], cookieParts[1]));
                            }
                        }
                        
                    }
                }
                else
                {
                    bodyBuilder.AppendLine(currLine);
                }
                

            }
          
            this.Body = bodyBuilder.ToString().TrimEnd('\r', '\n');
            DataParser(this.FormData, this.Body);
          




        }
        private void DataParser(IDictionary<string,string> output,string input)
        {
            var inputParts = input.Split("&", StringSplitOptions.RemoveEmptyEntries);
            foreach (var inputPart in inputParts)
            {
                var parameterParts = inputPart.Split("=", 2, StringSplitOptions.None);
                output.Add(
                    HttpUtility.UrlEncode(parameterParts[0]),
                    HttpUtility.UrlEncode(parameterParts[1]));
            }
        }
     
        public IList<Cookie> Cookies { get; set; }
        public IList<Header> Headers { get; set; }
        public HttpMethodType MethodType { get; set; }
        public string Path { get; set; }
        public HttpVersionType Version { get; set; }

        public string Body { get; set; }
        public IDictionary<string,string> SessionData { get; set; }
        public IDictionary<string,string> FormData { get; set; }
        public IDictionary<string,string> QueryData{ get; set; }


    }
}