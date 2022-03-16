using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Logika interakcji dla klasy OptimaLogin.xaml
    /// </summary>
    public partial class OptimaLogin : Page
    {
        public OptimaLogin()
        {
            InitializeComponent();

            Thread t = new Thread(() =>
            {
                

                SqlConnection c = Tools.CreateSqlConnection(Data.Config.Firm.Serwer, Data.MainDb.User, Data.MainDb.Password, Data.Config.Firm.DB);

                bool GetPass = false;

                if(c == null)
                     GetPass = true;

                if (GetPass)
                {
                    Windows.DbPass dbp = new Windows.DbPass();
                    dbp.ShowDialog();
                    if(dbp.Result == null)
                    {
                        MessageBox.Show("Nie można połączyć się z bazą danych", "Brak połączenia");
                        Environment.Exit(0);
                        return;
                    }
                    c = Tools.CreateSqlConnection(Data.Config.Firm.Serwer, dbp.Result.User, dbp.Result.Password, Data.Config.Firm.DB);
                    if (c == null)
                    {
                        
                        MessageBox.Show("Nie można połączyć się z bazą danych", "Brak połączenia");
                        Environment.Exit(0);
                        return;
                    }
                }

                Data.Sqlc = c;

                Dispatcher.Invoke(() =>
                {
                    OpeList.Items.Clear();
                });

                List<string> Ope = new List<string>();

                using (SqlCommand cm = new SqlCommand("SELECT [Ope_Kod] FROM [" + Data.Config.Database + "].[CDN].[Operatorzy]", c))
                {
                    using (SqlDataReader r = cm.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            Ope.Add(r.GetString(0));
                        }
                    }
                }

                Ope = Ope.OrderBy(x => x).ToList();

                foreach(string o in Ope)
                {
                    Dispatcher.Invoke(() =>
                    {
                        OpeList.Items.Add(new TextBlock { 
                            Text = o,
                            Tag = o,
                        });
                    });
                }

                Dispatcher.Invoke(() =>
                {
                    LoadingText.Visibility = Visibility.Collapsed;
                });

            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            
        }

        private void OpeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfirmButton.IsEnabled = OpeList.SelectedIndex != -1;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.Goto(new Select_DB());
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(Tools.Optima.Connect(((TextBlock)OpeList.SelectedItem).Text, Passwd.Password))
            {
                Tools.Goto(new Main.ImportExport());
            }
           
        }
    }
}
