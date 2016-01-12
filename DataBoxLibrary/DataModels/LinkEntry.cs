using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataBoxLibrary.DataModels
{
    /// <summary>
    /// Represents an entry that contains a set of links with a specific theme.
    /// </summary>
    [DataContract]
    public class LinkEntry : Entry
    {
        [DataMember]
        private List<LinkItem> _links = new List<LinkItem>();

        [DataMember]
        public string Description { get; set; }
        public List<LinkItem> Links 
        { 
            get
            {
                return _links;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkEntry"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public LinkEntry(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="link">The link.</param>
        public void AddLink(LinkItem link)
        {
            _links.Add(link);
        }

        /// <summary>
        /// Adds the link.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        public LinkItem AddLink(string name, string link)
        {
            LinkItem newLink = new LinkItem(name, link);
            _links.Add(newLink);
            return newLink;
        }

        /// <summary>
        /// Adds the links.
        /// </summary>
        /// <param name="links">The links.</param>
        public void AddLinks(IEnumerable<LinkItem> links)
        {
            foreach (LinkItem link in links)
            {
                _links.Add(link);
            }
        }
    }
}
