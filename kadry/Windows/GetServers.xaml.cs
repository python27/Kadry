using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
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
using System.Windows.Shapes;

namespace kadry.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy GetServers.xaml
    /// </summary>
    public partial class GetServers : Window
    {
        public string Result = null;
        public GetServers()
        {
            InitializeComponent();

            new Task(() =>
            {
                try
                {
                    List<string> Servers = GtSrvrs();
                    if (Servers.Count == 0)
                    {
                        LoadingText.Text = "Nie odnaleziono serwerów";
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            LoadingText.Visibility = Visibility.Collapsed;
                            foreach (string inst in Servers)
                            {
                                ServerList.Items.Add(new TextBlock
                                {
                                    Text = inst,
                                });
                            }
                        });
                    }
                }
                catch { }
            }).Start();
        }

        private List<string> GtSrvrs()
        {
            //// Retrieve the enumerator instance, and then retrieve the data sources.
            SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
            DataTable dtDatabaseSources = instance.GetDataSources();

            List<string> Instances = new List<string>();

            //// Populate the data sources into DropDownList.            
            foreach (DataRow row in dtDatabaseSources.Rows)
                if (!string.IsNullOrWhiteSpace(row["InstanceName"].ToString()))
                    Instances.Add(row["ServerName"].ToString() + "\\" + row["InstanceName"].ToString());

            return Instances;
        }

        private void SelectServer_Click(object sender, RoutedEventArgs e)
        {
            Result = ((TextBlock)ServerList.SelectedItem).Text;
            Close();
        }

        private void ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ServerList.SelectedIndex == -1)
            {
                SelectServer.IsEnabled = false;
            }
            else
            {
                SelectServer.IsEnabled = true;
            }
        }
    }
}
