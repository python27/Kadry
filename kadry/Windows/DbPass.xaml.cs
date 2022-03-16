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
    /// Logika interakcji dla klasy DbPass.xaml
    /// </summary>
    public partial class DbPass : Window
    {

        public Credentials Result;

        public DbPass()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Result = new Credentials
            {
                User = User.Text,
                Password = Password.Password,
            };
            Close();
        }
    }
}
