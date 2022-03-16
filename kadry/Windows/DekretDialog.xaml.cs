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
    public partial class DekretDialog : Window
    {
        public DekretDialogElement Result = null;

        private List<DekretDialogElement> Els;

        private List<DekretDialogElement> AllEls;

        private List<TextBlock> Items;

        private bool init = true;

        public DekretDialog(string Title, List<DekretDialogElement> Elements)
        {
            InitializeComponent();

            this.Title = Title != null ? Title : "";

            //Desc.Text = Description != null ? Description : "";

            Els = Elements.Where(x => x != null).ToList();
            AllEls = Elements.Where(x => x != null).ToList();

            InitYearCombo();

            SymbolSrch_TextChanged();

            init = false;
        }

        private void InitYearCombo()
        {
            //List<string> Years = new List<string>();
            //foreach(DekretDialogElement el in AllEls)
            //{
            //    if (!Years.Contains(el.Rok)) Years.Add(el.Rok);
            //}
            //List<TextBlock> Ys = new List<TextBlock> {
            //    new TextBlock{Text = "Wszystkie", Tag = "All"},
            //};

            //foreach(string y in Years)
            //{
            //    Ys.Add(new TextBlock { Text = y, Tag = y });
            //}

            //YearCombo.ItemsSource = null;
            //YearCombo.ItemsSource = Ys;
            //YearCombo.SelectedIndex = 0;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            Result = Els[Lista.SelectedIndex];
            Close();
        }

        private void ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Select.IsEnabled = Lista.SelectedIndex != -1;
        }

        private void ComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Select.IsEnabled = Lista.SelectedIndex != -1;
        }

        private void Lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Select.IsEnabled = Lista.SelectedIndex != -1;
        }

        private void SymbolSrch_TextChanged(object sender = null, TextChangedEventArgs e = null)
        {
            Els = AllEls.Where(x =>
            x.Text.IndexOf(SymbolSrch.Text, StringComparison.OrdinalIgnoreCase) >= 0 &&
            x.DateText.Contains(NazwaSrch.Text)
            ).ToList();

            Lista.ItemsSource = null;
            Lista.ItemsSource = Els;
        }
    }
}
