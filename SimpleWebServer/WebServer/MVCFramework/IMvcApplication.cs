using HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework
{
    public interface IMvcApplication
    {

        void Configure(List<Route> routeTable);

        void ConfigureServices();

    }
}
