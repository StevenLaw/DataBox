using DataBoxLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBox.ViewModels
{
    /// <summary><see cref="ObservableCollection{T}">ObservableCollection{T}'s</see> for the </summary>
    public class DataBoxViewModel
    {
        public ObservableCollection<Entry> Entries { get; set; } = new ObservableCollection<Entry>();
        //public ObservableCollection<Tag> Tags { get; set; } = new ObservableCollection<Tag>();
        //public ObservableCollection<IViewTagItem> TagTree { get; set; } = new ObservableCollection<IViewTagItem>();
        public TagTree TagTree { get; set; } = new TagTree();

        public void AddEntry(Entry entry)
        {
            Entries.Add(entry);
        }

        public void SetEntries(IEnumerable<Entry> entries)
        {
            foreach (Entry entry in entries)
            {
                Entries.Add(entry);
            }
        }
    }
}
