using CarShop.Data;
using CarShop.WebModels.Cars;
using CarShop.WebModels.Issues;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Controllers
{
   public  class IssuesController:Controller
    {
        private readonly ApplicationDbContext context;

        public IssuesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public HttpResponse CarIssues(string carId)
        {
            var car = context.Cars
                .Where(x => x.Id == carId)
                .Select(x => new CarViewModel
                {
                    Model = x.Model,
                    Id = x.Id,
                    Year = x.Year,
                    Issues = x.Issues.Select(i => new IssueInListViewModel
                    {
                        Id = i.Id,
                        Description = i.Description,
                        IsFixed = i.IsFixed
                    }).ToList()

                });

            return this.View(car);
        }
    }
}
