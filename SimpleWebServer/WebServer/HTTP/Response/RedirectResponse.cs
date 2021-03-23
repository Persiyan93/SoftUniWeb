using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP.Response
{
    public class RedirectResponse:HttpResponse
    {
        public RedirectResponse(string path)
        {
            this.responseCode = HttpResponseCode.Found;
            this.Headers.Add(new Header("Location", path));
            
        }
        
    }
}
