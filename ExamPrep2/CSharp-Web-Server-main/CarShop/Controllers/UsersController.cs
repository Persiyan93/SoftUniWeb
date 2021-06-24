using CarShop.WebModels.Users;
using MyWebServer.Controllers;
using MyWebServer.Http;
using CarShop.Services;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarShop.Data;
using CarShop.Data.Models;

namespace CarShop.Controllers
{
    public class UsersController:Controller
    {
       
        private readonly ApplicationDbContext context;
        private readonly IPasswordHasher passwordHasher;

        public UsersController(ApplicationDbContext context,IPasswordHasher passwordHasher)
        {
           
            this.context = context;
            this.passwordHasher = passwordHasher;
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterUserFormModel inputModel)
        {
            
            var (isValid,errors) = Validator.IsValid(inputModel);
            if (!isValid)
            {
                var errorsToString = errors.Select(x => x.ErrorMessage).ToList();
                return Error(errorsToString);
            }
            var isUserExist = context.Users.Any(x => x.Username == inputModel.Username);
            if (isUserExist )
            {
                return Error($"User with {inputModel.Username} username already exist.");
            }
            var user = new User
            {
                Username = inputModel.Username,
                Email = inputModel.Email,
                Password = passwordHasher.HashPassword(inputModel.Password),
                IsMechanic = inputModel.UserType == "Mechanic"

            };
            context.Users.Add(user);
            context.SaveChanges();

            return Redirect("/Users/Login");
        }

        [HttpPost]
        public HttpResponse Login(LoginUserFormModel inputModel)
        {
            var userId = context.Users
                .Where(x => x.Username == inputModel.Username && x.Password == passwordHasher.HashPassword(inputModel.Password))
                .Select(x => x.Id)
                .FirstOrDefault();
            if (userId==null)
            {
                return Error("Invalid username or password.");
            }
            this.SignIn(userId);
            return Redirect("/Cars/All");
        }
    }
}
