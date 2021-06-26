using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.ViewModels.Users
{
    public class RegisterInputModel:IValidatableObject
    {
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Password!=ConfirmPassword)
            {
                results.Add(new ValidationResult("Password and Confirm password must be the same."));
            }
            return results;

           
        }
    }
}
