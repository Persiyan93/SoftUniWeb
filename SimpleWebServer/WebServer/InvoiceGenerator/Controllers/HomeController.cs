using HTTP;
using MVCFramework;
using MVCFramework.HttpMethodsAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGenerator.Controllers
{
    class HomeController:Controller
    {
        [HttpGet("/")]
        public HttpResponse HomeIndex(HttpRequest request)
        {
           return this.View();
        }
    }
}
