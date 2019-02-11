using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataBoxLibrary.DataModels
{
    /// <summary>
    /// An abstract class that unifies various types of class allowing them to be 
    /// contained in a single data structure.
    /// </summary>
    [DataContract]
    public abstract class Entry
    {
        [DataMember]
        protected HashSet<Tag> _tags = new HashSet<Tag>();
        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        public HashSet<Tag> Tags 
        { 
            get
            {
                return _tags;
            }
        }
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Adds the tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        public bool AddTag(Tag tag)
        {
            return _tags.Add(tag);
        }

        /// <summary>
        /// Adds the tags.
        /// </summary>
        /// <param name="tags">The tags.</param>
        public int AddTags(IEnumerable<Tag> tags)
        {
            int count = 0;
            foreach (Tag tag in tags)
                if (_tags.Add(tag))
                    count++;
            return count;
        }

        /// <summary>
        /// Removes the tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="Tag" /> was successfully removed, <c>false</c> otherwise.
        /// </returns>
        public bool RemoveTag(Tag tag)
        {
            return _tags.Remove(tag);
        }

        /// <summary>
        /// Removes the tag where.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>
        /// The number of items removed.
        /// </returns>
        public int RemoveTagWhere(Predicate<Tag> where)
        {
            return _tags.RemoveWhere(where);
        }
    }
}
