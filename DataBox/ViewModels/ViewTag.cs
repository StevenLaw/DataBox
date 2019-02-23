using DataBoxLibrary.DataModels;

namespace DataBox.ViewModels
{
    /// <summary>Displays a <see cref="DataBoxLibrary.DataModels.Tag"/></summary>
    public class ViewTag: IViewTagItem
    {
        public Tag Tag { get; set; }
        /// <summary>Gets the parent.</summary>
        /// <value>The parent.</value>
        public ViewTagCategory Parent { get; }
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get => Tag.Name; }

        /// <summary>Initializes a new instance of the <see cref="ViewTag"/> class.</summary>
        /// <param name="parent">The parent.</param>
        public ViewTag(Tag tag, ViewTagCategory parent = null)
        {
            Tag = tag;
            Parent = parent;
        }
    }
}
