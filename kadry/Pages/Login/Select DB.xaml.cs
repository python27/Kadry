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
    /// Logika interakcji dla klasy Select_DB.xaml
    /// </summary>
    public partial class Select_DB : Page
    {
        public Select_DB()
        {
            InitializeComponent();
            new Task(() =>
            {
                List<Db> DBs = Tools.GetOptimaDbs();

                Dispatcher.Invoke(() =>
                {
                    DbList.Items.Clear();
                });

                foreach (Db db in DBs)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DbList.Items.Add(new TextBlock { Text = db.Name, Tag = db.Name });
                    });
                }

                Dispatcher.Invoke(() =>
                {
                    LoadingText.Visibility = Visibility.Collapsed;
                });
            }).Start();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Data.ConfigDbName = ((string)((TextBlock)DbList.SelectedItem).Tag);
            Data.Config.Database = Data.ConfigDbName;

            string firm = ((string)((TextBlock)FirmsList.SelectedItem).Tag);

            Data.Config.Firm = firms.First(x => x.Firma == firm);

            SqlConnection c2 = Tools.CreateSqlConnection(Data.Config.Firm.Serwer, Data.Config.UserSql, Data.Config.PasswordSql);
            if (c2 == null)
            {
                MessageBox.Show("Nie można połączyć się z bazą danych", "Błąd połączenia");
                return;
            }

            Data.FirmCn = c2;

            Tools.Config.Save();

            Tools.Goto(new OptimaLogin());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.Sqlc.Close();
            }
            catch { }
            Data.Sqlc = null;
            Tools.Goto(new ConnectToSql());
        }

        private List<Firm> firms = new List<Firm>();

        private void DbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ConfirmButton.IsEnabled = DbList.SelectedIndex != -1;
            if (DbList.SelectedIndex != -1)
            {
                string DB = ((string)((TextBlock)DbList.SelectedItem).Tag);
                using (SqlCommand c = new SqlCommand("SELECT [Baz_Nazwa] ,[Baz_NazwaBazy] ,[Baz_NazwaSerwera] FROM [" + DB + "].[CDN].[Bazy] where Baz_Nieaktywna <> 1", Data.Sqlc))
                {
                    using (SqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            firms.Add(new Firm
                            {
                                Firma = r[0].ToString(),
                                DB = r[1].ToString(),
                                Serwer = r[2].ToString(),
                            });
                        }
                    }
                }
                FirmsList.Items.Clear();

                firms = firms.OrderBy(x => x.Firma).ToList();

                foreach (Firm f in firms)
                {
                    FirmsList.Items.Add(new TextBlock
                    {
                        Text = f.Firma,
                        FontSize = 15,
                        Tag = f.Firma,
                    });
                }

                FirmsList.IsEnabled = true;
            }
            else
            {
                FirmsList.Items.Clear();
                FirmsList.IsEnabled = false;
            }
        }

        private void FirmsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfirmButton.IsEnabled = FirmsList.SelectedIndex != -1;
        }
    }
}