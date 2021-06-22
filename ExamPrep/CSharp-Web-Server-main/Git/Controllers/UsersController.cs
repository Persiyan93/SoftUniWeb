using Git.Data;
using Git.Data.Models;
using Git.Models.Users;
using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Controllers
{
    public class UsersController:Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext data;
        private readonly IPasswordHasher passwordHasher;

        public UsersController(IValidator validator ,GitDbContext data,IPasswordHasher passwordHasher)
        {
            this.validator = validator;
            this.data = data;
            this.passwordHasher = passwordHasher;
        }
        public HttpResponse Login()
        {
            if (User.IsAuthenticated)
            {
                return Redirect("/Repositories/All");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginInputModel inputModel)
        {
            var hashedPassword = passwordHasher.HashPassword(inputModel.Password);
            var userId = data.Users
                .Where(x => x.Username == inputModel.Username && x.Password == hashedPassword)
                .Select(x => x.Id)
                .FirstOrDefault();
            if (userId==null)
            {
                return Error("Invalid username or password!");
            }
            this.SignIn(userId);
            return Redirect("/Repositories/All");
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterUserInputModel inputModel)
        {
            var errors = validator.ValidateUser(inputModel);
            if (data.Users.Any(x=>x.Username==inputModel.Username))
            {
                errors.Add($"User with {inputModel.Username} username already exists.");
            }
            if (data.Users.Any(x=>x.Email==inputModel.Email))
            {
                errors.Add($"User with {inputModel.Email} e-mail already exist.");
            }
            if (errors.Count!=0)
            {
                this.Error(errors);
            }
            var user = new User
            {
                Username = inputModel.Username,
                Email = inputModel.Email,
                Password = this.passwordHasher.HashPassword(inputModel.Password)
            };
            data.Users.Add(user);
            data.SaveChanges();

            return Redirect("/Users/Login");

        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
