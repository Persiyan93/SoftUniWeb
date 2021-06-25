using BattleCards.Data;
using BattleCards.Models;
using BattleCards.Services;
using BattleCards.ViewModels.Cards;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Controllers
{
   public  class CardsController:Controller
    {
        private readonly ApplicationDbContext context;

        public CardsController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [Authorize]
        public HttpResponse Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Add(CardInputModel inputModel)
        {
            var (isValid,errors)=Validator.IsValid(inputModel);
            if (!isValid)
            {
                var errorMessages = errors.Select(x => x.ErrorMessage.ToString()).ToList();
                return this.Error(errorMessages);
            }
            var card = new Card
            {
                Name = inputModel.Name,
                ImageUrl = inputModel.Image,
                Keyword = inputModel.Keyword,
                Attack = inputModel.Attack,
                Health = inputModel.Health,
                Description=inputModel.Description

            };

            context.Cards.Add(card);

            card.UserCards.Add(new UserCard { UserId = this.User.Id });

          

            context.SaveChanges();

            return this.Redirect("/Cards/All");

          }

        [Authorize]
        public HttpResponse All()
        {
            var cards = context.Cards.ToList();
            return this.View(cards);
        }

        [Authorize]
        public HttpResponse Collection()
        {
            var cards = context.UserCards
                .Where(x => x.UserId == this.User.Id)
                .Select(x => x.Card)
                .ToList();
            return this.View(cards);
        }

        [Authorize]
        public HttpResponse RemoveFromCollection(string cardId)
        {
            var userCard = context.UserCards
                .FirstOrDefault(x => x.CardId == cardId && x.UserId == this.User.Id);
            if (userCard==null)
            {
                return this.BadRequest();
            }
            context.UserCards.Remove(userCard);
            context.SaveChanges();

            return this.Redirect("/Cards/Collection");
        }

        [Authorize]
        public HttpResponse AddToCollection(string cardId)
        {
            var usercard = context.UserCards
                .FirstOrDefault(x => x.CardId == cardId && x.UserId == User.Id);
            if (usercard!=null)
            {
                return this.Redirect("/Cards/All");
            }
            var userCard = new UserCard { UserId = this.User.Id, CardId = cardId };

            context.UserCards.Add(userCard);

            context.SaveChanges();
            return this.Redirect("/Cards/All");


        }
    }
}
