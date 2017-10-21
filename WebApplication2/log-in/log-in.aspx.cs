using System;
using System.Configuration;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Web.Security;
using System.Web.SessionState;
using System.Collections;
using NManif.App_Logic.Helpers.Active_Directory;
using NManif.Helpers;

public partial class Public_Log_On : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.IsAuthenticated)
        {
            //SiteLogin.RedirectToDefaultPage();
            // if they came to the page directly, ReturnUrl will be null.
            if (String.IsNullOrEmpty(Request["ReturnUrl"]))
            {
                /* in that case, instead of redirecting, I hide the login 
                   controls and instead display a message saying that are 
                   already logged in. */
                Response.Redirect("~/App_Resources/messages/Ja-Autenticado.aspx");
            }
            else
            {
                Response.Redirect("~/App_Resources/messages/Acesso-Negado.aspx");
            }
        }
        else
        {
            SetFocus(txtUserName);
        }

        /*
         * if (User.Identity.IsAuthenticated)
        {

            // if they came to the page directly, ReturnUrl will be null.
            if (String.IsNullOrEmpty(Request["ReturnUrl"]))
            {
                 /* in that case, instead of redirecting, I hide the login 
                    controls and instead display a message saying that are 
                    already logged in. 
            }
            else
            {
            Response.Redirect("~/AccessDenied.aspx");
            }*/

    }

    protected void ButtonLogOn_Click(object sender, EventArgs e)
    {
        if (String.IsNullOrEmpty(txtUserName.Value) || String.IsNullOrEmpty(txtPassword.Value))
            labelMessage.Text = ("Os campos |username| e |password| têm que estar preenchidos.");
        else
        {
            //Fazer os diferentes testes de acesso
            /* ********************* testar se o utilizador existe e está activo ******************/
            if (VerificaNim(txtUserName.Value)) // user existe e está activo
            {
                /* ********************* testar password na AD ******************/
                if (Ligacao.AutenticaUserAD(txtUserName.Value, txtPassword.Value)) //user autenticado na AD
                //if (true) 
                {
                    // Ir buscar o Nome com base no Nim e criar o nome de utilizador (nome|nim). Assim no cookie tenho acesso
                    // ao Nim para ir buscar o perfil no global.asax
                    //Se autenticado com sucesso
                    //Application.Lock();
                    //Application["TotalLoginUsers"] = (int)Application["TotalLoginUsers"] + 1;
                    //ArrayList aux = (ArrayList)Application["LoginUsers"];
                    //aux.Add(txtUserName.Value);
                    //Application["LoginUsers"] = aux;
                    //Arraylist Application["LoginUsers"]
                    //Application.UnLock();   
                    //"""""""""""""""""
                    string nomeNim = getNomeByNim(txtUserName.Value) + "|" + txtUserName.Value;
                    SiteLogin.PerformAuthentication(nomeNim, checkBoxRemember.Checked);
                    
                   
                }
                else
                {
                    labelMessage.Text = MessageFormatter.GetFormattedErrorMessage(
                        "<strong>Erro de Login!</strong><hr/>O username e/ou password estão incorrectas.<br/>" +
                        "Utilize as credenciais da RDE. Atenção à escrita e utilização de maiúsculas.");
                }
            }
            else
            {
                labelMessage.Text = MessageFormatter.GetFormattedErrorMessage(
                    "O utilizador não está parametrizado na aplicação ou está no estado inactivo. " +
                    "Contacte o Administrador...");
            }

        }
    }

    protected bool VerificaNim(string nim)
    {
        SqlConnection mConn = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["NMANIFConnectionString"].ConnectionString);
        try
        {
            mConn.Open();
            string sSQL = "select u.Activo from utilizador u where nim = '" + nim + "'";
            SqlCommand mComand = mConn.CreateCommand();
            mComand.CommandText = sSQL;
            SqlDataReader dr = mComand.ExecuteReader();
            return true;
            /*if (dr.HasRows) //user existe. Verificar se está activo
            {
                dr.Read();
                if ((bool)dr["Activo"])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }*/
        }
        catch (SqlException ex)
        {
            labelMessage.Text = MessageFormatter.GetFormattedErrorMessage(
                "<strong>Erro de acesso à Base de dados</strong><hr/>" + ex.Message);
            return false;
        }
        finally
        {
            mConn.Close();
        }
    }

    protected string getNomeByNim(string nim)
    {
        System.Data.SqlClient.SqlConnection mConn = new System.Data.SqlClient.SqlConnection(
                        ConfigurationManager.ConnectionStrings["NMANIFConnectionString"].ConnectionString);
        try
        {
            mConn.Open();
            string sSQL = "select u.Posto+' '+u.Nome as nome " +
                "from utilizador u where u.Nim = '" + nim + "'";
            System.Data.SqlClient.SqlCommand mComand = mConn.CreateCommand();
            mComand.CommandText = sSQL;
            System.Data.SqlClient.SqlDataReader dr = mComand.ExecuteReader();
            dr.Read();
            return dr["nome"].ToString();
        }
        catch (SqlException ex)
        {
            labelMessage.Text = MessageFormatter.GetFormattedErrorMessage(
                "<strong>Erro de acesso à Base de dados</strong><hr/>" + ex.Message);
            return string.Empty;
        }
        finally
        {
            mConn.Close();
        }
    }

}


