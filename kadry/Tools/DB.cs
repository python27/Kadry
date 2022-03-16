using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kadry
{
    public static partial class Tools
    {
        /// <summary>
        /// Pobieranie listy baz danych należących do Optimy
        /// </summary>
        /// <returns></returns>
        public static List<Db> GetOptimaDbs()
        {
            List<Db> DBs = new List<Db>();

            List<string> AllDBs = new List<string>();

            using (SqlCommand c = new SqlCommand("SELECT name FROM master.sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')", Data.Sqlc))
            {
                using (SqlDataReader r = c.ExecuteReader())
                {
                    while (r.Read())
                    {
                        AllDBs.Add(r.GetString(0));
                    }

                }
            }

            string filter = "SELECT TOP (1) * FROM [CDN].[Bazy]";

            foreach (string db in AllDBs)
            {
                try
                {
                    new SqlCommand("use [" + db + "]", Data.Sqlc).ExecuteNonQuery();
                    using (SqlCommand c = new SqlCommand(filter, Data.Sqlc))
                    {
                        using (SqlDataReader r = c.ExecuteReader())
                        {
                            DBs.Add(new Db(db));
                        }
                    }
                }
                catch
                {
                }
            }

            //foreach(Db db in DBs)
            //{
            //    using (SqlCommand c = new SqlCommand())
            //}

            return DBs;
        }

        /// <summary>
        /// Tworzy połączenie z serwerem SQL na podstawie podanych danych
        /// </summary>
        /// <param name="Server">AdresSerwera\\Instancja</param>
        /// <param name="User">Użytkownik Sql</param>
        /// <param name="Password">Hasło SQL</param>
        /// <param name="DB">Nazwa używanej bazy danych (Opcjonalne)</param>
        /// <returns>Połączenie SQL</returns>
        public static SqlConnection CreateSqlConnection(string Server, string User, string Password, string DB = null)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder() { DataSource = Server, TrustServerCertificate = true, IntegratedSecurity = false, ConnectTimeout = 15, UserID = User, Password = Password };

            if (!String.IsNullOrWhiteSpace(DB))
                scsb.InitialCatalog = DB;

            string CS = (scsb).ConnectionString;
            SqlConnection c = new SqlConnection(CS);

            try
            {
                c.Open();
                c.Close();
                c.Open();
                return c;
            }
            catch
            {
                return null;
            }
        }
    }
}
