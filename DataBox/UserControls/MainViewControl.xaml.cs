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

        protected DataBoxLibrary.DataBox Databox
        {
            get => (Application.Current.MainWindow as MainWindow)?.Databox;
            set
            {
                if (Application.Current.MainWindow is MainWindow mw)
                {
                    mw.Databox = value;
                }
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
    }
}
