using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kadry
{
    public static partial class Data
    {
        public static SqlConnection Sqlc;

        public static SqlConnection FirmCn;
        public static string DbName;
        public static string ConfigDbName;

        public static string Firma { get; set; }

        public static class MainDb
        {
            public static string User;
            public static string Password;
        }
    }
}
