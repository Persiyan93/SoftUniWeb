using CarShop.WebModels.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.WebModels.Cars
{
   public class CarViewModel
    {
        public string Id { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public ICollection<IssueInListViewModel> Issues { get; set; }


    }
}
