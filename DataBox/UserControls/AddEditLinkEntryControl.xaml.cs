using DataBox.Core;
using DataBoxLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private ObservableCollection<LinkItem> links = new ObservableCollection<LinkItem>();

        public ObservableCollection<LinkItem> Links { get => links; }

        protected void InitializeTagControl()
        {
            tagsMain.CurrentAutocomplete = (value) =>
            {
                return GlobalData.Databox.TagList.
                    Select(x => x.Display).
                    Where(x => x.Contains(value)).ToArray();
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEditLinkEntryControl"/> class.
        /// </summary>
        public AddEditLinkEntryControl()
        {
            InitializeComponent();
            InitializeTagControl();

            btnAdd.IsDefault = true;
            btnEdit.Visibility = Visibility.Collapsed;

            lvLinks.ItemsSource = links;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddEditLinkEntryControl"/> class.
        /// </summary>
        /// <param name="linkEntry">The link entry.</param>
        public AddEditLinkEntryControl(LinkEntry linkEntry)
        {
            InitializeComponent();
            InitializeTagControl();

            btnAdd.Visibility = Visibility.Collapsed;
            btnEdit.IsDefault = true;

            txtName.Text = linkEntry.Name;
            txtDescription.Text = linkEntry.Description;
            foreach (LinkItem link in linkEntry.Links)
            {
                links.Add(new LinkItem(link.Name, link.Link));
            }
            lvLinks.ItemsSource = links;
            
            tagsMain.AddTags(linkEntry.Tags.Select(x => x.Display).ToArray());
            

            this.linkEntry = linkEntry;
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            LinkEntry entry = GlobalData.Databox.NewLinkEntry(txtName.Text, txtDescription.Text);
            //entry.AddLinks(links);
            entry.SyncLinks(links);
            GlobalData.Databox.SyncEntryTags(entry, tagsMain.Tags);
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ccMain.Content = new MainViewControl();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ccMain.Content = new MainViewControl();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                linkEntry.Name = txtName.Text;
                linkEntry.Description = txtDescription.Text;
                linkEntry.SyncLinks(links);
                GlobalData.Databox.SyncEntryTags(linkEntry, tagsMain.Tags);
                mainWindow.ccMain.Content = new MainViewControl();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAddLink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnAddLink_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => { links.Add(new LinkItem("", "")); });
            //links.Add(new LinkDisplay());
        }

        /// <summary>
        /// Handles the Click event of the btnDeleteLink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void btnDeleteLink_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is LinkItem link)
            {
                Links.Remove(link);
            }
        }
    }
}
