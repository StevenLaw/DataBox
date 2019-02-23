using System.Collections.Generic;

namespace DataBox.ViewModels
{
    /// <summary>Displays a tag category for a <see cref="DataBoxLibrary.DataModels.Tag">Tag's</see> category.</summary>
    public class ViewTagCategory: IViewTagItem
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; }
        /// <summary>Gets or sets the tags.</summary>
        /// <value>The tags.</value>
        public List<ViewTag> Tags { get; set; } = new List<ViewTag>();

        /// <summary>Initializes a new instance of the <see cref="ViewTagCategory"/> class.</summary>
        /// <param name="name">The name.</param>
        public ViewTagCategory(string name)
        {
            Name = name;
        }
    }
}
