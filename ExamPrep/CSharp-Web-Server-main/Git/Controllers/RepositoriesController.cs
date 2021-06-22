using Git.Data;
using Git.Data.Models;
using Git.Models.Repositories;
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
    public class RepositoriesController:Controller
    {
        private readonly GitDbContext data;
        private readonly IValidator validator;

        public RepositoriesController(GitDbContext data,IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }


        [Authorize]
        public HttpResponse Create() => this.View();
        
        [Authorize]
        [HttpPost]
        public HttpResponse Create(RepositoryInputModel inputModel)
        {
            
            var errors = validator.ValidateRepository(inputModel);
            if (errors.Count!=0)
            {
                return Error(errors);
            }
            var repository = new Repository
            {
                Name = inputModel.Name,
                CreatedOn = DateTime.Now,
                IsPublic = inputModel.RepositoryType == "Public",
                OwnerId = User.Id

            };



            data.Repositories.Add(repository);

            data.SaveChanges();

            return Redirect("/Repositories/All");
        }

        public HttpResponse All()
        {
            var repositoriesQuery = data.Repositories.AsQueryable();

            if (this.User.IsAuthenticated)
            {
                repositoriesQuery.Where(x => x.IsPublic || x.OwnerId == User.Id);
            }
            else
            {
                repositoriesQuery = repositoriesQuery.Where(x => x.IsPublic);
            }

            var repositories = repositoriesQuery.Select(x => new RepositoryListingViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Owner = x.Owner.Username,
                CreatedOn = x.CreatedOn.ToString("F"),
                CommitsCount = x.Commits.Count()


            })
             .ToList();
            return View(repositories);

        }
    }
}
