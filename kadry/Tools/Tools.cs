using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace kadry
{
    public static partial class Tools
    {
        public static void Goto(Page Page)
        {
            if(Data.Mw != null)
            {
                Data.Mw.MainFrame.Navigate(Page);
            }
        }
        public static void Log(string Log)
        {

            try
            {
                if (!Directory.Exists(Path.Combine(Data.BaseDir, "Logs"))) Directory.CreateDirectory(Path.Combine(Data.BaseDir, "Logs"));
                //Próba zapisania logu do domyślnego pliku
                string LogPath = Path.Combine(
                    Path.Combine(Data.BaseDir, "Logs"),
                    $"Log-{DateTime.Now.ToString("dd-MM-yyyy")}.txt"
                    );

                if (!File.Exists(LogPath)) File.Create(LogPath).Close();

                //Dopisywanie loga na końcu pliku
                using (FileStream f = File.Open(LogPath, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(f))
                    {
                        sw.WriteLine(String.Format("[{0}] {1}", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), Log));
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }


    }
}
