using HTTP;
using InvoiceGenerator.ViewModels;
using MVCFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGenerator.Controllers
{
    class InvoiceController:Controller
    {
        public HttpResponse InvoiceList(HttpRequest request)
        {
            var model = new Test();
            return this.View(model);
        }
    }
}
