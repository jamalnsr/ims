using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMRegister
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (Request["kxk"] == null)
                return;

            if (WebSession.Session.ContainsKey(Request["kxk"]))
            {
                WebSession.Session.Remove(Request["kxk"]);
            }
        }
    }
}