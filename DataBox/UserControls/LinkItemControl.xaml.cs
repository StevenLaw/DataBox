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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataBox.UserControls
{
    /// <summary>
    /// Interaction logic for LinkItemControl.xaml
    /// </summary>
    public partial class LinkItemControl : UserControl
    {
        public LinkItemControl(LinkEntry entry)
        {
            InitializeComponent();
            lblTitle.Content = entry.Name;
            tblkDescription.Text = entry.Description;
            foreach (LinkItem link in entry.Links)
            {
                tblkLinks.Inlines.Add(new Hyperlink(new Run(link.Name))
                {
                    CommandParameter = link.Link,
                    Command = new LinkCommand()
                });
                tblkLinks.Inlines.Add(new Run(Environment.NewLine));
            }
        }
    }
}
