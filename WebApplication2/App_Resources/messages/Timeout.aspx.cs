using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NManif.Helpers;

namespace NManif.App_Resources.messages
{
    public partial class Timeout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                Response.Redirect("~/default.aspx");
            }
            else
            {
                labelMessage.Text = MessageFormatter.GetFormattedNoticeMessage(
                                "<strong>Atenção!</strong><hr/>Devido ao tempo de inactividade terá que se autenticar novamente<br/>");
            }
        }
    }
}