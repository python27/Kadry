using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Interop;
using System.Drawing;

namespace kadry
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            LogoImg.Source = BitmapToImgSource(Resource1.Computernet);


            Data.BaseDir = AppDomain.CurrentDomain.BaseDirectory;


            //CheckUpdate();

            bool GenNewConf = true;

            Data.AppPath = AppDomain.CurrentDomain.BaseDirectory;
            Data.Mw = this;
            Tools.Config.Load();
            if (Data.Config != null)
            {
                GenNewConf = false;
                SqlConnection c = Tools.CreateSqlConnection(Data.Config.ServerSql, Data.Config.UserSql, Data.Config.PasswordSql);
                if (c == null)
                {
                    MessageBox.Show("Nie można połączyć się z serwerem SQL", "Błąd połączenia");
                    GenNewConf = true;
                }

                SqlConnection c2 = Tools.CreateSqlConnection(Data.Config.Firm.Serwer, Data.Config.UserSql, Data.Config.PasswordSql);
                if (c2 == null)
                {
                    MessageBox.Show("Nie można połączyć się z serwerem SQL", "Błąd połączenia");
                    GenNewConf = true;
                }

                Data.MainDb.User = Data.Config.UserSql;
                Data.MainDb.Password = Data.Config.PasswordSql;

                Data.Sqlc = c;

                Data.FirmCn = c2;

                Data.ConfigDbName = Data.Config.Database;

                Data.DbName = Data.Config.Firm.DB;

                Dispatcher.Invoke(() =>
                {
                    Tools.Goto(new Pages.Login.OptimaLogin());
                    return;
                });
            }
                
            if (GenNewConf)
            {
                Data.Config = new Config();
                Tools.Goto(new Pages.Login.ConnectToSql());
            }
            


            
        }

        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ImageSource BitmapToImgSource(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
