using BattleCards.Data;
using BattleCards.Models;
using BattleCards.Services;
using BattleCards.ViewModels.Users;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Controllers
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
        public HttpResponse Register(RegisterInputModel inputModel)
        {
            var (isValid, errors) = Validator.IsValid(inputModel);
            if (!isValid)
            {
                var errorMessages = errors.Select(x => x.ErrorMessage.ToString()).ToList();
                return this.Error(errorMessages);
            }
            var userExist = context.Users
                .Any(x => x.Username == inputModel.Username);
            if (userExist)
            {
                return this.View($"User with {inputModel.Username}  username already exist.");
            }
            var user = new User
            {
                Username = inputModel.Username,
                Email = inputModel.Email,
                Password = passwordHasher.HashPassword(inputModel.Password)
            };

            context.Users.Add(user);
            context.SaveChanges();

            return this.Redirect("/Users/Login");

                

        }

        [HttpPost]
       public HttpResponse Login(LoginInputModel inputModel)
        {
            var user = context.Users
                .FirstOrDefault(x => x.Username == inputModel.Username
                && x.Password == passwordHasher.HashPassword(inputModel.Password));
            if (user==null)
            {
                return this.Error("Invalid username or password");
            }

            this.SignIn(user.Id);

             return this.Redirect("/Cards/All");
        }

        [Authorize]
        public  HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");

        }
    }
}
