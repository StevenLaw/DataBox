using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

namespace DataBox.Windows
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            Title = versionInfo.ProductName;
            lblTitle.Content = versionInfo.ProductName;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(versionInfo.ProductVersion);
            sb.AppendLine($"Version: {versionInfo.LegalCopyright}");
            sb.Append(versionInfo.Comments);
            txtInfo.Text = sb.ToString();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
