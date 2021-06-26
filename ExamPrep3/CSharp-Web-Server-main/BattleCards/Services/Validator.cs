using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Services
{
    public  class Validator:IValidator
    {
        public Validator()
        {
            validationResults = new List<ValidationResult>();
        }
        private ICollection<ValidationResult> validationResults;
        public  bool IsValid(object model)
        {
            var validationContext = new ValidationContext(model);
           

            var result = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(model, validationContext, validationResults, true);

            return (result);
        }
        public ICollection<string> GetErrorMessages()
        {
          
           var  errorMessages = validationResults.Select(x => (string)x.ErrorMessage).ToList();
            return errorMessages;
        }
    }
}
