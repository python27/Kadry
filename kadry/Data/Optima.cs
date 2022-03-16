using CDNBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kadry
{
    public static partial class Data
    {
        public static bool Beta;

        public static bool NoUpdate { get; set; }
        public static string BaseDir { get; set; }

        public static class Optima
        {
            public static IApplication Application = null;
            public static ILogin Login = null;
        }
    }
}
