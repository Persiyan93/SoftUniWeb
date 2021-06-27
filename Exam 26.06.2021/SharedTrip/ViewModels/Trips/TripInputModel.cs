using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTrip.ViewModels.Trips
{
    public   class TripInputModel
    {
        [Required]
        public string StartPoint { get; set; }

        [Required]
        public string EndPoint { get; set; }

        [Required]
        public string DepartureTime { get; set; }

        public string ImagePath { get; set; }

        [Required]
        [Range(2, 6, ErrorMessage = "Seats must be between 2 and ")]
        public int Seats { get; set; }

        [Required]
        [MaxLength(80)]
        public string Description { get; set; }
    }
}
