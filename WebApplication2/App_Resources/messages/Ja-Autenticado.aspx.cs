using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NManif.Helpers;

namespace NManif.App_Resources.messages
{
    public partial class Ja_Autenticado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                labelMessage.Text = MessageFormatter.GetFormattedNoticeMessage(
                            "<strong>Atenção!</strong><hr/>Já se encontra autenticado<br/>");
            }
        }
    }
}