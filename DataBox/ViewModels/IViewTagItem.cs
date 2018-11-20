using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBox.ViewModels
{
    /// <summary>Interface item for the tag <see cref="System.Windows.Controls.TreeView"/></summary>
    public interface IViewTagItem
    {
        /// <summary>
        ///   <para>
        ///  Gets or sets the name.
        /// </para>
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
    }
}
