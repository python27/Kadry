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
    /// Logika interakcji dla klasy ErrorsList.xaml
    /// </summary>
    public partial class ErrorsList : Window
    {
        private Models.ErrorListViewModel model = new Models.ErrorListViewModel();
        public ErrorsList(string title, string message, IEnumerable<string> errors)
        {
            DataContext = model;
            InitializeComponent();

            Title = title;
            model.Message = message;
            model.Errors = string.Join("\n\n", errors);
            
        }

        private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            model.MessageHeight = ((TextBlock)sender).ActualHeight;
        }

        private void TextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            model.ErrorsHeight = ((TextBox)sender).ActualHeight;
        }
    }
}
