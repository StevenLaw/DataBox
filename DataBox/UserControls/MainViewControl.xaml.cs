using DataBox.Core;
using DataBox.ViewModels;
using DataBoxLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for MainViewControl.xaml
    /// </summary>
    public partial class MainViewControl : UserControl
    {
        private DataBoxViewModel model = new DataBoxViewModel();

        public bool ItemSelected { get => lvMain.SelectedItem != null; }

        protected DataBoxLibrary.DataBox Databox
        {
            get => GlobalData.Databox;
            set
            {
                GlobalData.Databox = value;
            }
        }


        /// <summary>Loads this instance.</summary>
        private void Load()
        {
            if (Databox == null)
                MessageBox.Show(Application.Current.MainWindow,
                    "File is not a Data Box file.",
                    "The selected file was not the correct format.",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            else
            {
                //}
                model.SetEntries(Databox.Entries);
                model.TagTree.Tags = Databox.TagList;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="MainViewControl"/> class.</summary>
        public MainViewControl()
        {
            InitializeComponent();

            DataContext = model;

            Load();
        }

        public Entry DeleteSelected()
        {
            if (ItemSelected)
            {
                if (lvMain.SelectedItem is Entry entry && model.Entries.Remove(entry))
                {
                    return entry;
                }
            }
            return null;
        }

        /// <summary>Handles the RequestNavigate event of the Hyperlink control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RequestNavigateEventArgs"/> instance containing the event data.</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (e.Uri.IsAbsoluteUri)
            {
                Process.Start(e.Uri.AbsoluteUri);
            } else
            {
                try
                {
                    Process.Start($"http://{e.Uri.ToString()}");
                }
                catch (Exception)
                {
                    MessageBox.Show(App.Current.MainWindow, 
                        $"{e.Uri.ToString()} is not a valid url", 
                        "URL Error", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Handles the SelectedItemChanged event of the TreeTags control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedPropertyChangedEventArgs{System.Object}"/> instance containing the event data.</param>
        private void TreeTags_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            CollectionViewSource.GetDefaultView(lvMain.ItemsSource).Refresh();
        }

        /// <summary>
        /// Handles the Filter event of the CollectionViewSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FilterEventArgs"/> instance containing the event data.</param>
        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (e.Item is Entry entry)
            {
                if (treeTags.SelectedItem == null)
                    e.Accepted = true;
                else if (treeTags.SelectedItem is ViewTag tag && entry.Tags.Contains(tag.Tag))
                    e.Accepted = true;
                else if (treeTags.SelectedItem is ViewTagCategory category && entry.Tags.Any(x => x.Category == category.Name))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }
    }
}
