﻿using HTTP;
using MVCFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGenerator.Controllers
{
    class CompanyController:Controller
    {
        public HttpResponse CreateCompany()
        {
            return this.View();
        }
    }
}
