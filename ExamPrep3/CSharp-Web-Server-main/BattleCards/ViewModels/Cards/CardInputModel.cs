using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.ViewModels.Cards
{
   public  class CardInputModel
    {
        [Required]
        [MaxLength(15)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Keyword { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Attack { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Health { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
