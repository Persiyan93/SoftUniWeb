﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP
{
    interface IHttpServer
    {
        Task StratAsync();

        Task ResetAsync();

        void Stop();
    }
}
