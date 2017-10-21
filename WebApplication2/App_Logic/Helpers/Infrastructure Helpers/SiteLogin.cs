/****************** Copyright Notice *****************
 
This code is licensed under Microsoft Public License (Ms-PL). 
You are free to use, modify and distribute any portion of this code. 
The only requirement to do that, you need to keep the developer name, as provided below to recognize and encourage original work:

=======================================================
   
Architecture Designed and Implemented By:
Mohammad Ashraful Alam
Microsoft Most Valuable Professional, ASP.NET 2007 – 2011
Twitter: http://twitter.com/AshrafulAlam | Blog: http://blog.ashraful.net | Portfolio: http://www.ashraful.net
   
*******************************************************/

namespace NManif.Helpers
{
    using System.Web.Security;
    using System.Web;
    using System.Collections;


    /// <summary>
    /// Design and Architecture: Mohammad Ashraful Alam [http://www.ashraful.net]
    /// </summary>
    public sealed class SiteLogin
    {
        public static void PerformAuthentication(string userName, bool remember)
        {
            FormsAuthentication.RedirectFromLoginPage(userName, remember);

            if (System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"] == null ||
                System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"] == "/secured/log-in/log-out.aspx")
                RedirectToDefaultPage();
            else
                System.Web.HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"].ToString());

        }

        public static void PerformAdminAuthentication(string userName, bool remember)
        {
            FormsAuthentication.RedirectFromLoginPage(userName, remember);

            if (System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"] == null ||
                System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"] == "/secured/log-in/log-out.aspx")
                RedirectToAdminDefaultPage();
            else
                System.Web.HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Request.QueryString["ReturnUrl"].ToString());

        }

        /// <summary>
        /// Redirects the current user based on role -> Envio ambos para a página inicial do site.
        /// </summary>
        public static void RedirectToDefaultPage()
        {
            System.Web.HttpContext.Current.Response.Redirect("~/default.aspx");
        }

        public static void RedirectToAdminDefaultPage()
        {
            System.Web.HttpContext.Current.Response.Redirect("~/default.aspx");
        }

        public static void LogOff()
        {
            //string nim = HttpContext.Current.User.Identity.Name.ToString();
            //System.Web.HttpContext.Current.Application.Lock();
            //System.Web.HttpContext.Current.Application["TotalLoginUsers"] =
            //    (int)System.Web.HttpContext.Current.Application["TotalLoginUsers"] - 1;
            //ArrayList aux = (ArrayList)System.Web.HttpContext.Current.Application["LoginUsers"];
            //aux.Remove(nim);
            //System.Web.HttpContext.Current.Application["LoginUsers"] = aux;
            //System.Web.HttpContext.Current.Application.UnLock();
            
            
            
            
            // Put user code to initialize the page here
            FormsAuthentication.SignOut();

            //// Invalidate roles token
            //Response.Cookies[Globals.UserRoles].Value = "";
            //Response.Cookies[Globals.UserRoles].Path = "/";
            //Response.Cookies[Globals.UserRoles].Expires = new System.DateTime(1999, 10, 12);

            //Set the current user as null
            System.Web.HttpContext.Current.User = null;


        }
    }
}