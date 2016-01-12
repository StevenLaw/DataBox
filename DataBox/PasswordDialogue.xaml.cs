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

namespace DataBox
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PasswordDialogue : Window
    {
        public string Password { get; set; }

        public PasswordDialogue()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Password = txtPassword.Password;
            DialogResult = true;
            Close();
        }
    }
}
