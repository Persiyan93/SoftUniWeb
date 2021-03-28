using HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework.HttpMethodsAttributes
{
    class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute()
        {

        }
        public HttpPostAttribute(string path)
            :base(path)
        {

        }
        public override HttpMethodType Type => HttpMethodType.Post;
    }
}
