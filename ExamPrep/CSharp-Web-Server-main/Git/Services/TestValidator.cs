using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Services
{
    public static class TestValidator
    {
        public static bool IsValid(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResult = new List<ValidationResult>();

            var result = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, validationContext, validationResult, true);

            return result;
        }
    }
}
