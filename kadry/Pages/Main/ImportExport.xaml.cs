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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kadry.Pages.Main
{
    /// <summary>
    /// Logika interakcji dla klasy ImportExport.xaml
    /// </summary>
    public partial class ImportExport : Page
    {
        public ImportExport()
        {
            InitializeComponent();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.Goto(new Menu());
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.Goto(new Export());
        }

        private void ImportPlikButton_Click(object sender, RoutedEventArgs e)
        {
            Tools.Goto(new ImportPlik());
        }
    }
}
