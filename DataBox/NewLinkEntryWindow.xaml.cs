using DataBoxLibrary.DataModels;
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
    /// Interaction logic for NewLinkEntryWindow.xaml
    /// </summary>
    public partial class NewLinkEntryWindow : Window
    {
        public LinkEntry Entry { get; set; }

        public NewLinkEntryWindow(Window owner = null)
        {
            InitializeComponent();
            Owner = owner;
            txtName.Focus();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                DialogResult = true;
            MainWindow main = Application.Current.MainWindow as MainWindow;
            if (main != null)
            {
                main.Databox.NewLinkEntry(txtName.Text, txtDescription.Text);
                main.LoadEntries();
            }
            
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Interop.ComponentDispatcher.IsThreadModal)
                DialogResult = false;
            Close();
        }
    }
}
