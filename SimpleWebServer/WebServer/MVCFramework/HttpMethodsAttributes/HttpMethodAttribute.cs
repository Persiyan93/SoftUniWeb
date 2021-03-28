using HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework.HttpMethodsAttributes
{
    public abstract class HttpMethodAttribute:Attribute
    {
        protected HttpMethodAttribute()
        {

        }
        protected HttpMethodAttribute(string path)
        {
            this.Path = path;
        }
        abstract public HttpMethodType Type { get; }
         public string Path { get; set; }
    }
}
