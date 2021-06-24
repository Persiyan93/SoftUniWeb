using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.WebModels.Users
{
   public  class RegisterUserFormModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        public string UserType { get; set; }
    }
}
