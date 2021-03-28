using HTTP;
using InvoiceGenerator.Controllers;
using MVCFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGenerator
{
    public class Startup : IMvcApplication
    {
        public void Configure(IList<Route> routeTable)
        {
            //routTable.Add(new Route("/",HttpMethodType.Get,new HomeController().HomeIndex));
            //routTable.Add(new Route("/Companies/CreateCompany",HttpMethodType.Get,new CompaniesController().CreateCompany));
            //routTable.Add(new Route("/Companies/CreateCompany",HttpMethodType.Post,new CompaniesController().PostCreateCompany));
            //routTable.Add(new Route("/Invoice/AllInvoice",HttpMethodType.Get,new InvoiceController().InvoiceList));
            Console.WriteLine(routeTable.Count);
            foreach (var route in routeTable)
            {
                Console.WriteLine(route.MethodType.ToString()+"   =>"+ route.Path);
            }
        }

        public void ConfigureServices()
        {
    
            //var db = new InvoiceDbContext();
            //db.Database.EnsureCreated();
        }
    }
}
