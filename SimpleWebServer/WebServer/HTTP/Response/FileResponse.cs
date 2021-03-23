using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.Response
{
    public class FileResponse:HttpResponse
    {
        public FileResponse(byte[] fileContent,string contentType)
        {

            this.responseCode = HttpResponseCode.Ok;
            this.Body = fileContent;
            this.Headers.Add(new Header("Content-Type", contentType));
            this.Headers.Add(new Header("Content-Length ", this.Body.Length.ToString()));
        }
    }
}
