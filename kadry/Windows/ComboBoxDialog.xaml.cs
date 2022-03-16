using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy ComboBoxDialog.xaml
    /// </summary>
    public partial class ComboBoxDialog : Window
    {

        public ComboBoxDialogElement Result = null;

        private List<ComboBoxDialogElement> Els;

        private List<TextBlock> Items;

        public ComboBoxDialog(string Title, string Description, List<ComboBoxDialogElement> Elements)
        {
            InitializeComponent();
            this.Title = Title != null ? Title : "";
            Desc.Text = Description != null ? Description : "";

            Els = Elements;
            Items = new List<TextBlock>();

            foreach(ComboBoxDialogElement el in Els)
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Confirm.IsEnabled = ComboList.SelectedIndex != -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Result = Els[ComboList.SelectedIndex];
            Close();
        }
    }
}
