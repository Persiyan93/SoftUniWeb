using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework
{
    public interface IViewEngine
    {
        string GetHtml(string temlate ,object model );
    }
}
