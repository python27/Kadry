using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace kadry
{
    public static partial class Tools
    {
        public static class Config
        {
            private static string ConfigPath { get {
                    return Path.Combine(Data.AppPath, "Config.json");
                } }

            public static void Load() {
                try
                {
                    if (File.Exists(ConfigPath))
                    {
                        string json = File.ReadAllText(ConfigPath);
                        Data.Config = JsonConvert.DeserializeObject<kadry.Config>(json);
                    }
                }
                catch { }
            }

            public static void Save()
            {
                try
                {
                    if (File.Exists(ConfigPath))
                    {
                        File.Delete(ConfigPath);
                    }

                    File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Data.Config, Formatting.Indented));
                }
                catch { }
            }
        }
    }
}
