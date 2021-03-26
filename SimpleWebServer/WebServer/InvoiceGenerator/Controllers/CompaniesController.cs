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
    }
}
