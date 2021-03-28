using HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework.HttpMethodsAttributes
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute()
        {

        }
        public HttpGetAttribute(string path)
            :base(path)
        {

        }
        public override HttpMethodType Type =>HttpMethodType.Get;

        public  string Path { get ; set ; }
    }
}
