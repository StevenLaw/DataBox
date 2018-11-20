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
                //foreach (Entry entry in Databox.Entries)
                //{
                //    if (entry is LinkEntry lEntry)
                //    {

                //    }
                //}
                model.SetEntries(Databox.Entries);
                model.SetTags(Databox.TagList);

                //var root = new ObservableCollection<ViewModels.IViewTagItem>
                //{
                //    new ViewModels.ViewTagCategory("Test") { Tags = new List<ViewModels.ViewTag>
                //    {
                //        new ViewModels.ViewTag("Test 1"),
                //        new ViewModels.ViewTag("Test 2")
                //    }
                //    },
                //    new ViewModels.ViewTag("Root Test")
                //};
                //treeTags.ItemsSource = root;
            }
        }

        public MainViewControl()
        {
            InitializeComponent();

            DataContext = model;

            Load();
        }

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

    public class DataBoxViewModel
    {
        public ObservableCollection<Entry> Entries { get; set; } = new ObservableCollection<Entry>();
        public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
        public ObservableCollection<IViewTagItem> TagTree { get; set; } = new ObservableCollection<IViewTagItem>();

        public void SetEntries(IEnumerable<Entry> entries)
        {
            foreach (Entry entry in entries)
            {
                Entries.Add(entry);
            }
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            foreach (Tag tag in tags)
            {
                Tags.Add(tag);
            }
            var groupedTags = tags.GroupBy(x => x.Category);
            foreach (var category in groupedTags.Where(x => !string.IsNullOrWhiteSpace(x.Key)).OrderBy(x => x.Key))
            {
                ViewTagCategory viewTagCategory = new ViewTagCategory(category.Key);
                foreach (Tag tag in category.OrderBy(x => x.Name))
                {
                    viewTagCategory.Tags.Add(new ViewTag(tag.Name));
                }
                TagTree.Add(viewTagCategory);
            }
            foreach (Tag tag in groupedTags.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Key)).OrderBy(x => x.Name))
            {
                TagTree.Add(new ViewTag(tag.Name));
            }
        }
    }
}
