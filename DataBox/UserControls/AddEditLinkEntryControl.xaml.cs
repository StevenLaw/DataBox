using DataBoxLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddEditLinkEntryControl.xaml
    /// </summary>
    public partial class AddEditLinkEntryControl : UserControl
    {
        private LinkEntry linkEntry;

        private ObservableCollection<LinkDisplay> links = new ObservableCollection<LinkDisplay>();
        //private ObservableCollection<string> tags = new ObservableCollection<string>();

        public AddEditLinkEntryControl()
        {
            InitializeComponent();

            btnAdd.IsDefault = true;
            btnEdit.Visibility = Visibility.Collapsed;
        }

        public AddEditLinkEntryControl(LinkEntry linkEntry)
        {
            InitializeComponent();

            btnAdd.Visibility = Visibility.Collapsed;
            btnEdit.IsDefault = true;

            txtName.Text = linkEntry.Name;
            txtDescription.Text = linkEntry.Description;
            foreach (LinkItem link in linkEntry.Links)
            {
                links.Add(new LinkDisplay(link.Name, link.Link));
            }
            lvLinks.ItemsSource = links;
            
            tagsMain.AddTags(linkEntry.Tags.Select(x => x.Display).ToArray());
            tagsMain.CurrentAutocomplete = (value) =>
            {
                return (App.Current.MainWindow as MainWindow)?.
                    Databox.
                    TagList.
                    Where(x => x.Display.Contains(value)).
                    Select(x => x.Display).ToArray();
            };

            this.linkEntry = linkEntry;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ccMain.Content = new MainViewControl();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ccMain.Content = new MainViewControl();
            }
        }

        private void btnAddLink_Click(object sender, RoutedEventArgs e)
        {
            links.Add(new LinkDisplay());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class LinkDisplay
    {
        private string OldName;
        private string OldLink;
        public string Name { get; set; }
        public string Link { get; set; }
        public bool IsNew { get; set; }
        public LinkDisplay()
        {
            IsNew = true;
        }
        public LinkDisplay(string name, string link)
        {
            OldName = name;
            Name = name;
            OldLink = link;
            Link = link;
            IsNew = false;
        }
    }
}
