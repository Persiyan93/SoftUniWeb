using Git.Data;
using Git.Data.Models;
using Git.Models.Commits;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Controllers
{
   public  class CommitsController:Controller
    {
        private readonly GitDbContext data;

        public CommitsController(GitDbContext data)
        {
            this.data = data;
        }
        [Authorize]
        public HttpResponse Create(string id)
        {
            var repository = data.Repositories
                  .Where(x => x.Id == id)
                  .Select(x => new CommitToRepositoryViewModel{ Name = x.Name, Id = x.Id })
                  .FirstOrDefault();
              
             if (repository==null)
            {
                return this.BadRequest();

            }
            return this.View(repository);
        }

        [HttpPost]
        [Authorize] 
        public HttpResponse Create(CreateCommitInputModel inputModel)
        {
            var isRepositoryExist = data.Repositories.Any(x => x.Id == inputModel.Id);
            if (!isRepositoryExist)
            {
                return this.BadRequest();
            }
            var commit = new Commit
            {
                Description = inputModel.Description,
                RepositoryId = inputModel.Id,
                CreatorId = User.Id,
                CreatedOn = DateTime.Now
            };

            data.Commits.Add(commit);
            data.SaveChanges();
            return this.Redirect("/Repositories/All");

        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = data.Commits
                    .Where(x => x.CreatorId == User.Id)
                    .Select(x => new CommitInListViewModel
                    {
                        Repository = x.Repository.Name,
                        CreatedOn = x.CreatedOn.ToString("F"),
                        Description = x.Description,
                        Id = x.Id
                    })
                    .ToList();
            return this.View(commits);
        }

        [Authorize]
        public HttpResponse Delete(string id)
        {
            var commit = data.Commits.FirstOrDefault(x => x.Id == id && x.CreatorId == User.Id);

            if (commit ==null)
            {
                return this.BadRequest();

            }
            data.Commits.Remove(commit);
            data.SaveChanges();
            return this.Redirect("/Commits/All");
        }


    }
}
