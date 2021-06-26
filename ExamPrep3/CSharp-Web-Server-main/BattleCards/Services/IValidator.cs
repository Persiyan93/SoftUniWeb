using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Services
{
    public interface IValidator
    {
        public bool IsValid(object model);

        public ICollection<string>GetErrorMessages();
    }
}
