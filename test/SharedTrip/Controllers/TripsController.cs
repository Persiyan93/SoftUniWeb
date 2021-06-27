using Microsoft.EntityFrameworkCore;
using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Models;
using SharedTrip.Services;
using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.Controllers
{
    public class TripsController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IValidator validator;

        public TripsController(ApplicationDbContext context,IValidator validator)
        {
            this.context = context;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse Add()
        {
            return this.View();
        }

        [Authorize]
        public HttpResponse All()
        {
            var trips = context.Trips
                .Select(x => new TripsInListViewModel
                {
                    StartPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    Id = x.Id,
                    DepartureTime = x.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    Seats = x.Seats-x.UserTrips.Count

                })
                .ToList();
            return this.View(trips);
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(TripInputModel inputModel)
        {
            DateTime date;
            var isValidDatetime = DateTime
                .TryParseExact(inputModel.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            var isValid = validator.IsValid(inputModel);
            if (!isValid||!isValidDatetime)
            {
                return this.Redirect("/Trips/Add");
            }
           

            var trip = new Trip
            {
                StartPoint = inputModel.StartPoint,
                EndPoint = inputModel.EndPoint,
                DepartureTime = date,
                ImagePath = inputModel.ImagePath,
                Description = inputModel.Description,
                Seats = inputModel.Seats
            };

            context.Trips.Add(trip);

            context.SaveChanges();

            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            var trip = context.Trips
                .Where(x => x.Id == tripId)
                .Select(x => new TripViewModel
                {
                    Id = x.Id,
                    StartPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    Description = x.Description,
                    DepartureTime = x.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    Seats = x.Seats - x.UserTrips.Count(),
                    ImagePath = x.ImagePath

                })
                .FirstOrDefault();
            return this.View(trip);


        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var trip = context.Trips.Include(x=>x.UserTrips).FirstOrDefault(x => x.Id == tripId);
            if (trip==null)
            {
                return this.NotFound();

            }
           
            if (trip.UserTrips.Any(x=>x.UserId==User.Id)|| trip.UserTrips.Count == trip.Seats)
            {
                return this.Redirect($"/Trips/Details?tripId={tripId}");
            }

            var userToTrip = new UserTrip
            {
                UserId = this.User.Id,
                TripId = tripId
            };
            trip.UserTrips.Add(userToTrip);

            context.SaveChanges();

            return this.Redirect("/Trips/All");
        }
    }
}
