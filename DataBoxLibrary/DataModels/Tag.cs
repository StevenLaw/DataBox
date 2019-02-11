using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DataBoxLibrary.DataModels
{
    /// <summary>
    /// A tag identifying the content of an entry.
    /// </summary>
    [DataContract]
    public sealed class Tag : IEquatable<Tag>
    {
        [DataMember]
        private string _name;
        [DataMember]
        private string _category;
        //[DataMember]

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                return _name;
            }
        }
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category 
        { 
            get
            {
                return _category;
            }
        }

        /// <summary>
        /// Gets the display.
        /// </summary>
        /// <value>
        /// The display.
        /// </value>
        public string Display 
        { 
            get
            {
                if (string.IsNullOrWhiteSpace(_category))
                    return _name;
                else
                    return _category + ":" + _name;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="subcategory">The sub category.</param>
        public Tag(string name, string category = "")
        {
            _name = name;
            _category = category;
        }

        #region Equality

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Tag other)
        {
            if (other == null) return false;
            else return (string.Equals(other.Name, _name, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(other.Category, _category, StringComparison.InvariantCultureIgnoreCase));
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
            if (obj is null) return false;
            else if (ReferenceEquals(this, obj)) return false;
            else if (obj.GetType() != GetType()) return false;
            else return Equals(obj as Tag);
        }

        /// <summary>
        /// Checks the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool CheckString(string value)
        {
            return (ToString().Equals(value));
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Tag t1, Tag t2)
        {
            if (t1 is null)
                return t2 is null;
            return t1.Equals(t2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Tag t1, Tag t2)
        {
            if (t1 is null)
                return t2 is null;
            return !t2.Equals(t2);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _name.GetHashCode() ^ _category.GetHashCode();
        }

        #endregion
    }
}
