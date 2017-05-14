using System;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Web.UI.WebControls;

namespace Siderite.Web.Adapters
{
    /// <summary>
    /// I have found no way to set the parameters of a ControlAdapter
    /// so this is the abstract class from which one can inherit 
    /// differently behaving ListControlUpdatePanelFixAdapters
    /// </summary>
    public abstract class ListControlUpdatePanelFixAdapterBase : ControlAdapter
    {
        /// <summary>
        /// Set it to false if you use old ASP.Net callbacks
        /// </summary>
        public abstract bool RemoveInitCallback { get; }

        /// <summary>
        /// Minimum number of items in the ListControl after 
        /// which the transformation is being applied
        /// Set it to 0 to apply it on any ListBox or DDL
        /// </summary>
        public abstract int MinItemCount { get; }

        protected override void OnLoad(EventArgs e)
        {
            var lc = Control as ListControl;
            //The control must be a list control on a page
            if (lc != null&&lc.Page!=null)
            {
                // The control must have at least MinItemCount items
                if (lc.Items.Count > MinItemCount)
                {
                    var sm = ScriptManager.GetCurrent(lc.Page);
                    // the page must have a ScriptManager defined
                    if (sm != null)
                    {
                        // define the fix javascript function
                        var script =
                            @"function FixLargeListControl(clientId) {
var select=document.getElementById(clientId); 
if (select) {
    var stub=document.createElement('input');
    stub.type='hidden';
    stub.id=select.id;
    stub.name=select.name;
    stub._behaviors=select._behaviors;
    var val=new Array();
    for (var i=0; i<select.options.length; i++)
       if (select.options[i].selected)
       {
           val[val.length]=select.options[i].value;
       }
    stub.value=val.join(',');
    select.parentNode.replaceChild(stub,select);
}
};";
                        ScriptManager.RegisterClientScriptBlock(lc.Page, lc.Page.GetType(), "FixLargeListControl",
                                                                script,
                                                                true);
                        // apply the function when disposing the element
                        script = string.Format("FixLargeListControl('{0}');", lc.ClientID);
                        sm.RegisterDispose(lc, script);
                        // remove the useless callback init function
                        if (RemoveInitCallback)
                        {
                            script = @"WebForm_InitCallback=function() {};";
                            ScriptManager.RegisterStartupScript(lc.Page, lc.Page.GetType(), "removeWebForm_InitCallback",
                                                                script, true);
                        }
                    }
                }
            }
            base.OnLoad(e);
        }

    }
}
