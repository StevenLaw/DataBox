using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBox.ViewModels
{
    /// <summary>Displays a <see cref="DataBoxLibrary.DataModels.Tag"/></summary>
    public class ViewTag: IViewTagItem
    {
        /// <summary>Gets the parent.</summary>
        /// <value>The parent.</value>
        public ViewTagCategory Parent { get; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Initializes a new instance of the <see cref="ViewTag"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="parent">The parent.</param>
        public ViewTag(string name, ViewTagCategory parent = null)
        {
            Name = name;
            Parent = parent;
        }
    }
}
