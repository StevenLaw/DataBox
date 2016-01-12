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
        public void AddTag(Tag tag)
        {
            _tags.Add(tag);
        }

        /// <summary>
        /// Adds the tags.
        /// </summary>
        /// <param name="tags">The tags.</param>
        public void AddTags(IEnumerable<Tag> tags)
        {
            foreach (Tag tag in tags)
                _tags.Add(tag);
        }
    }
}
