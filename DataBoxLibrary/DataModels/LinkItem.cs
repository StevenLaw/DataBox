using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataBoxLibrary.DataModels
{
    /// <summary>
    /// Represents a link in a <see cref="LinkEntry"/>.
    /// </summary>
    [DataContract]
    public class LinkItem : Entry, IEquatable<LinkItem>
    {
        [DataMember]
        public string Link { get; set; }

        public int Hash { get => GetHashCode(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="link">The link.</param>
        public LinkItem(string name, string link)
        {
            Name = name;
            Link = link;
        }

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(LinkItem other)
        {
            return other != null &&
                Link == other.Link &&
                Name == other.Name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is LinkItem item &&
                Link == item.Link &&
                Name == item.Name;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return 924860401 + 
                EqualityComparer<string>.Default.GetHashCode(Link) + 
                EqualityComparer<string>.Default.GetHashCode(Name);
        }

        #endregion
    }
}
