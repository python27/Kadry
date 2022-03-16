using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kadry
{
    public class Db
    {
        public Db() { }
        public Db(string Name)
        {
            this.Name = Name;
        }
        public Db(string Name, string Version)
        {
            this.Name = Name;
            this.Version = Version;
        }
        public string Name { get; set; } = "";
        public string Version { get; set; } = "";
    }

    public class Credentials
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}
