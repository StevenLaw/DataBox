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
        private LinkEntry entry;

        private void LoadEntry()
        {
            lblTitle.Content = entry.Name;
            txtTitle.Text = entry.Name;
            tblkDescription.Text = entry.Description;
            txtDescription.Text = entry.Description;
            foreach (LinkItem link in entry.Links)
            {
                tblkLinks.Inlines.Add(new Hyperlink(new Run(link.Name))
                {
                    CommandParameter = link.Link,
                    Command = new LinkCommand()
                });
                tblkLinks.Inlines.Add(new Run(Environment.NewLine));
            }
            lvLinks.ItemsSource = entry.Links;
            tagTags.ClearTags();
            tagTags.AddTags(entry.Tags.Select(x => x.Display).ToArray());
        }

        public LinkItemControl(ref LinkEntry entry)
        {
            InitializeComponent();

            this.entry = entry;
            LoadEntry();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            lblTitle.Visibility = Visibility.Collapsed;
            txtTitle.Visibility = Visibility.Visible;
            tblkDescription.Visibility = Visibility.Collapsed;
            txtDescription.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Collapsed;
            btnUpdate.Visibility = Visibility.Visible;
            tagTags.IsEnabled = true;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            lblTitle.Visibility = Visibility.Visible;
            txtTitle.Visibility = Visibility.Collapsed;
            tblkDescription.Visibility = Visibility.Visible;
            txtDescription.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
            btnUpdate.Visibility = Visibility.Collapsed;
            tagTags.IsEnabled = false;

            entry.Name = txtTitle.Text;
            entry.Description = txtDescription.Text;
            var tags = tagTags.Tags;
            var db = (App.Current.MainWindow as MainWindow)?.Databox;
            foreach (string tag in tags)
            {
                if (!entry.Tags.Any(x => x.CheckString(tag)))
                {
                    var split = tag.Split(':');
                    Tag newTag;
                    if (split.Length == 2)
                    {
                        if (db.TagExists(split[1], split[0]))
                            newTag = db.GetTagsByName(split[1], split[0]).First();
                        else
                            newTag = db.NewTag(split[1], split[0]);
                    }
                    else if(split.Length > 2)
                    {
                        string tagName = String.Join(" - ", split.Skip(1).ToArray());
                        if (db.TagExists(tagName, split[0]))
                            newTag = db.GetTagsByName(tagName, split[0]).First();
                        else
                            newTag= db.NewTag(tagName, split[0]);
                    }
                    else
                    {
                        if (db.TagExists(tag))
                            newTag = db.GetTagsByName(tag).First();
                        else
                            newTag = db.NewTag(tag);
                    }
                    entry.AddTag(newTag);
                }
            }
            var removedTags = new List<Tag>();
            foreach (Tag tag in entry.Tags)
            {
                if (!tags.Contains(tag.Display))
                    removedTags.Add(tag);
            }
            foreach (Tag tag in removedTags)
            {
                entry.Tags.Remove(tag);
            }
            LoadEntry();
            MainWindow main = (App.Current.MainWindow as MainWindow);
            main?.SetChangeMade(true);
            main?.LoadTags();
        }
    }
}
