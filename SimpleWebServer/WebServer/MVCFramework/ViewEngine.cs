using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework
{
    public class ViewEngine : IViewEngine
    {
        private string template;
        public ViewEngine(string template)
        {
            this.template = template;
        }
        public string GetHtml(object model)
        {
            var cSharpCode = GenerateCSharpCode(this.template);
            var code = @$"Using System;
                using System.Text;
                using System.Linq;
                using System.Collections.Generic;
                using SIS.MvcFramework;
                public class AppViewCode : IView
                  {{
                         public string GetHtml(object model, string user)
                   {{
                            var html=new stringBuilder()
                             {cSharpCode}
                            return html.toString();
                     }}
                    }}  ";

            Iview view = GetInstanceFromCode(code);
            return view.GetHtml();
        }

        private Iview GetInstanceFromCode(string code)
        {
            
        }

        private string GenerateCSharpCode(string template)
        {
            var code = new StringBuilder();




            return code.ToString();
        }

        public string GetHtml(string temlate)
        {
            throw new NotImplementedException();
        }
    }
}
