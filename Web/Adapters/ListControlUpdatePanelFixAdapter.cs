using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siderite.Web.Adapters
{
    /// <summary>
    /// Fixes the slow UpdatePanel asynchronous postback when large 
    /// listboxes or dropdownlists are on the page
    /// </summary>
    // Use it in a Browser file like this:
    // <adapter controlType ="System.Web.UI.WebControls.ListBox" adapterType="Siderite.Web.Adapters.ListControlUpdatePanelFixAdapter" />
    // <adapter controlType ="System.Web.UI.WebControls.DropDownList" adapterType="Siderite.Web.Adapters.ListControlUpdatePanelFixAdapter" />
    public class ListControlUpdatePanelFixAdapter : ListControlUpdatePanelFixAdapterBase
    {
        public override bool RemoveInitCallback
        {
            get { return true; }
        }

        public override int MinItemCount
        {
            get { return 50; }
        }
    }
}
