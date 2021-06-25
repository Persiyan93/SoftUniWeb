using MyWebServer.Controllers;
using MyWebServer.Http;

namespace BattleCards.Controllers
{
  

    public class HomeController : Controller
    { 
        public HttpResponse Index()
        {
            if (User.IsAuthenticated)
            {
                return this.Redirect("/Cards/All");
            }
            return this.View();
        }
      
    }
}