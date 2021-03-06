using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Models.Users
{
    public class RegisterUserInputModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        [MaxLength(6)]
        [MinLength(4)]
        [Required]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
