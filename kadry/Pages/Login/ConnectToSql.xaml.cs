using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
using System.Windows.Shapes;

namespace kadry.Pages.Login
{
    /// <summary>
    /// Logika interakcji dla klasy ConnectToSql.xaml
    /// </summary>
    public partial class ConnectToSql : Page
    {
        public ConnectToSql()
        {
            InitializeComponent();

            
            
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            new Task(() =>
            {
                
                string Srv = null;
                string Usr = null;
                string Psw = null;
                Dispatcher.Invoke(() =>
                {
                    Srv = ServerName.Text;
                    Usr = User.Text;
                    Psw = Password.Password;
                });

                if (Srv == null || Usr == null || Psw == null) return;

                SqlConnection c = Tools.CreateSqlConnection(Srv, Usr, Psw);
                if (c == null)
                {
                    MessageBox.Show("Nie można połączyć się z serwerem SQL", "Błąd połączenia");
                    return;
                }

                Data.MainDb.User = Usr;
                Data.MainDb.Password = Psw;

                Data.Sqlc = c;

                Data.Config.ServerSql = Srv;
                Data.Config.UserSql = Usr;
                Data.Config.PasswordSql = Psw;

                Dispatcher.Invoke(() =>
                {
                    Tools.Goto(new Select_DB());
                });
                

            }).Start();
        }

        private void GetServersButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.GetServers gs = new Windows.GetServers();
            gs.ShowDialog();
            if(gs.Result != null)
            {
                ServerName.Text = gs.Result;
            }
        }
    }
}
