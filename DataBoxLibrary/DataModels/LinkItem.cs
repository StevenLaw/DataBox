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
    public class LinkItem : Entry
    {
        [DataMember]
        public string Link { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="link">The link.</param>
        public LinkItem(string name, string link)
        {
            this.Name = name;
            this.Link = link;
        }
    }
}
