using HTTP;
using MVCFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGenerator.Controllers
{
    class CompaniesController:Controller
    {
        public HttpResponse CreateCompany(HttpRequest request)
        {
            return this.View();
        }
        
        public HttpResponse PostCreateCompany(HttpRequest request)
        {
            foreach (var data in request.FormData)
            {
                Console.Write(data.Key + "  =>" + data.Value);
            }
            return null;
        }
    }
}
