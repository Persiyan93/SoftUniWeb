
using Git.Models.Repositories;
using Git.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Services
{
    public class Validator : IValidator
    {
        public ICollection<string> ValidateRepository(RepositoryInputModel inputModel)
        {
            var errors = new List<string>();
            if (inputModel.Name.Length<3||inputModel.Name.Length>10)
            {
                errors.Add("Length of repositoy's name must be between 3 and 10 symbols.");
            }
            if (inputModel.RepositoryType!="Public"&&inputModel.RepositoryType!="Private")
            {
                errors.Add($"Repository type can be either Private or Public");
            }
            return errors;
        }

        public ICollection<string> ValidateUser(RegisterUserInputModel input)
        {
            var errors = new List<string>();
            if (input.Username==null||input.Username.Length<GlobalConstants.UsernameMinLength||input.Username.Length>GlobalConstants.UsernameMaxLength)
            {
                errors.Add("Invalid username!");
            }
           if (input.Email==null)
            {
                errors.Add($"Email {input.Email} is not a valid e-mail address!");
            }
            if (input.Password.Length<GlobalConstants.PasswordMinLength||input.Password.Length>GlobalConstants.PasswordMaxLength)
            {
                errors
                    .Add($"The provided password is not valid.It must be between {GlobalConstants.PasswordMinLength} and {GlobalConstants.PasswordMaxLength}");
            }
            if (input.Password!=input.ConfirmPassword)
            {
                errors.Add("Password  and  Confirmed password are different!");
            }
            if (input.Password.Any(x => x == ' '))
            {
                errors.Add($"The provided password cannot contain whitespaces.");
            }
            return errors;
           
        }
    }
}
