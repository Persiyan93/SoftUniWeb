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
        public void Configure(IList<Route> routTable)
        {
            routTable.Add(new Route("/",HttpMethodType.Get,new HomeController().HomeIndex));
            routTable.Add(new Route("/Companies/CreateCompany",HttpMethodType.Get,new CompaniesController().CreateCompany));
            routTable.Add(new Route("/Companies/CreateCompany",HttpMethodType.Post,new CompaniesController().PostCreateCompany));
        }

        public void ConfigureServices()
        {
    
            //var db = new InvoiceDbContext();
            //db.Database.EnsureCreated();
        }
    }
}
