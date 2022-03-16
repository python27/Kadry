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
    public partial class WyplataDialog : Window
    {
        public WyplataDialogElement Result = null;

        private List<WyplataDialogElement> Els;

        private List<WyplataDialogElement> AllEls;

        private List<TextBlock> Items;

        private bool init = true;

        public WyplataDialog(string Title, List<WyplataDialogElement> Elements)
        {
            InitializeComponent();

            this.Title = Title != null ? Title : "";

            
            //Desc.Text = Description != null ? Description : "";

            Els = Elements.Where(x => x != null).ToList();
            AllEls = Elements.Where(x => x != null).ToList();

            InitYearCombo();

            SearchBox_TextChanged();

            init = false;
        }

        private void InitYearCombo()
        {
            List<string> Years = new List<string>();
            foreach(WyplataDialogElement el in AllEls)
            {
                if (!Years.Contains(el.Rok)) Years.Add(el.Rok);
            }
            List<TextBlock> Ys = new List<TextBlock> { 
                new TextBlock{Text = "Wszystkie", Tag = "All"},
            };

            foreach(string y in Years)
            {
                Ys.Add(new TextBlock { Text = y, Tag = y });
            }

            YearCombo.ItemsSource = null;
            YearCombo.ItemsSource = Ys;
            YearCombo.SelectedIndex = 0;
        }

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

            if(YearCombo.SelectedItem == null || ((TextBlock)YearCombo.SelectedItem).Tag.ToString() == "All")
            {
                Els = AllEls.Where(x => x.Text.ToUpper().Contains(SearchBox.Text.ToUpper())).ToList();
            }
            else
            {
                Els = AllEls.Where(
                        x => 
                        x.Text.ToUpper().Contains(SearchBox.Text.ToUpper()) &&
                        x.Rok == ((TextBlock)YearCombo.SelectedItem).Tag.ToString()
                    ).ToList();
            }

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

        private void YearCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (init) return;
            SearchBox_TextChanged();


        }
    }
}
