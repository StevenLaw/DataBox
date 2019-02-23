using DataBoxLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataBox.ViewModels
{
    /// <summary>
    /// Observable hierarchical collection for the tag tree built on a <see cref="HashSet{IViewTagItem}."/>
    /// </summary>
    /// <seealso cref="System.Collections.ObjectModel.ObservableCollection{DataBox.ViewModels.IViewTagItem}" />
    public class TagTree : ObservableCollection<IViewTagItem>
    {
        private HashSet<Tag> _tags;
        public HashSet<Tag> Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                SetTags();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagTree"/> class.
        /// </summary>
        public TagTree() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="TagTree"/> class.
        /// </summary>
        /// <param name="tags">The tags.</param>
        public TagTree(HashSet<Tag> tags)
        {
            _tags = tags;
            SetTags();
        }

        /// <summary>Sets the tags.</summary>
        private void SetTags()
        {
            Clear();
            var groupedTags = _tags.GroupBy(x => x.Category);
            foreach (var category in groupedTags.Where(x => !string.IsNullOrWhiteSpace(x.Key)).OrderBy(x => x.Key))
            {
                ViewTagCategory viewTagCategory = new ViewTagCategory(category.Key);
                foreach (Tag tag in category.OrderBy(x => x.Name))
                {
                    viewTagCategory.Tags.Add(new ViewTag(tag));
                }
                Add(viewTagCategory);
            }
            if (groupedTags.Any(x => string.IsNullOrWhiteSpace(x.Key)))
            {
                foreach (Tag tag in groupedTags.First(x => string.IsNullOrWhiteSpace(x.Key)).OrderBy(x => x.Name))
                {
                    Add(new ViewTag(tag));
                }
            }
        }

        /// <summary>Adds the tag.</summary>
        /// <param name="tag">The tag.</param>
        /// <returns><c>true</c> if the tag was succesfully added, <c>false</c> otherwise.</returns>
        public bool AddTag(Tag tag)
        {
            bool result = _tags.Add(tag);
            SetTags();
            return result;
        }

        /// <summary>Removes the tag.</summary>
        /// <param name="tag">The tag.</param>
        /// <returns>
        ///   <c>true</c> if the tag was removed, <c>false</c> otherwise.
        /// </returns>
        public bool RemoveTag(Tag tag)
        {
            bool result = _tags.Remove(tag);
            SetTags();
            return result;
        }

        /// <summary>Removes the tag where.</summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The number of <see cref="Tag">Tags</see> affected.</returns>
        public int RemoveTagWhere(Predicate<Tag> predicate)
        {
            int result = _tags.RemoveWhere(predicate);
            SetTags();
            return result;
        }
    }
}
