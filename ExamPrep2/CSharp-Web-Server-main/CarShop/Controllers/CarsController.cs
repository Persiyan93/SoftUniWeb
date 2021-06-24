using CarShop.Data;
using CarShop.Data.Models;
using CarShop.WebModels.Cars;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Controllers
{
   public  class CarsController:Controller
    {
        private readonly ApplicationDbContext context;

        public CarsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [Authorize]
        public HttpResponse All()
        {
            var cars = context.Cars.AsQueryable();
            var user = context.Users.FirstOrDefault(x => x.Id == User.Id);
            if (user.IsMechanic)
            {
                cars = cars.Where(x => x.Issues.Count != 0);
            }
            else
            {
                cars = cars.Where(x => x.OwnerId == user.Id);
            }
            var resultCars = cars.Select(x => new CarsInListViewModel
            {
                Model = x.Model,
                Id = x.Id,
                PlateNumber = x.PlateNumber,
                RemainingIssues = x.Issues.Where(i => i.IsFixed == false).Count(),
                FixedIssues = x.Issues.Where(i => i.IsFixed == true).Count(),
                PictureUrl=x.PictureUrl

            }).ToList();
            return this.View(resultCars);
        }

        [Authorize]
        public HttpResponse Add()
        {

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(CarInputModel inputModel)
        {
            var car = new Car
            {
                Model = inputModel.Model,
                PlateNumber = inputModel.PlateNumber,
                Year = inputModel.Year,
                PictureUrl = inputModel.Image,
                OwnerId = User.Id
            };
            context.Cars.Add(car);

            context.SaveChanges();

            return Redirect("/Cars/All");

        }
    }
}
