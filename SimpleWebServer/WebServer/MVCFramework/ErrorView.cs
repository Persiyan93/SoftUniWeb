using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework
{
    class ErrorView : IView
    {
        public string GetHtml(string message)
        {
            return message;
        }

        public string GetHtml()
        {
            throw new NotImplementedException();
        }

        public string GetHtml(object model)
        {
            throw new NotImplementedException();
        }
    }
}
