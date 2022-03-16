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
    public partial class ListBoxDialog : Window
    {
        public ComboBoxDialogElement Result = null;

        private List<ComboBoxDialogElement> Els;

        private List<ComboBoxDialogElement> AllEls;

        private List<TextBlock> Items;

        public ListBoxDialog(string Title, string Description, List<ComboBoxDialogElement> Elements)
        {
            InitializeComponent();

            this.Title = Title != null ? Title : "";

            //if(Description != null && Description.Trim() != "")
            //{
            //    this.Description.Text = Description;
            //    DescRow.Height = new GridLength(this.Description.ActualHeight + 10);
            //}
            //Desc.Text = Description != null ? Description : "";

            Els = Elements.Where(x => x != null).ToList();
            AllEls = Elements.Where(x => x != null).ToList();

            SearchBox_TextChanged();


        }

        //private List<string> GtSrvrs()
        //{
        //    //// Retrieve the enumerator instance, and then retrieve the data sources.
        //    SqlDataSourceEnumerator instance = SqlDataSourceEnumerator.Instance;
        //    DataTable dtDatabaseSources = instance.GetDataSources();

        //    List<string> Instances = new List<string>();

        //    //// Populate the data sources into DropDownList.            
        //    foreach (DataRow row in dtDatabaseSources.Rows)
        //        if (!string.IsNullOrWhiteSpace(row["InstanceName"].ToString()))
        //            Instances.Add(row["ServerName"].ToString() + "\\" + row["InstanceName"].ToString());

        //    return Instances;
        //}

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            Result = Els[ComboList.SelectedIndex];
            Close();
        }

        private void ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Select.IsEnabled = ComboList.SelectedIndex != -1;
        }

        private void ComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Select.IsEnabled = ComboList.SelectedIndex != -1;
        }

        private void SearchBox_TextChanged(object sender = null, TextChangedEventArgs e = null)
        {

            Els = AllEls.Where(x => x.Text.ToUpper().Contains(SearchBox.Text.ToUpper())).ToList();

            Items = new List<TextBlock>();

            foreach (ComboBoxDialogElement el in Els)
            {
                Items.Add(new TextBlock
                {
                    Text = el.Text,
                    Tag = el.Id,
                    FontSize = 16,
                });
            }

            ComboList.ItemsSource = null;
            ComboList.ItemsSource = Items;
        }
    }
}
