using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

public partial class portfolio : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        con.Open();
        Label1.Text = "";

    }
    protected void FormView2_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        GridView1.DataBind();
    }

    protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
    {
        TextBox ValorTextBox = (TextBox)FormView1.FindControl("TextBox1");
        DropDownList ValorTextBox2 = (DropDownList)FormView1.FindControl("DropDownList2");
        TextBox ValorTextBox3 = (TextBox)FormView1.FindControl("NomeTextBox");
        TextBox ValorTextBox4 = (TextBox)FormView1.FindControl("ApelidoTextBox");
        TextBox ValorTextBox5 = (TextBox)FormView1.FindControl("IdadeTextBox");
        DropDownList ValorTextBox6 = (DropDownList)FormView1.FindControl("DropDownList1");
        if (!ValorTextBox.Text.Equals(string.Empty) && !ValorTextBox2.Text.Equals(string.Empty))
        {
            Label1.Text = "<div>NÃO PODE INSERIR INFORMAÇÃO NOS DOIS ÚLTIMOS CAAMPOS</div>";
            e.Cancel = true;
        }
        else if (ValorTextBox2.Text.Equals(string.Empty) || ValorTextBox3.Text.Equals(string.Empty) || ValorTextBox4.Text.Equals(string.Empty) || ValorTextBox5.Text.Equals(string.Empty) || ValorTextBox6.Text.Equals(string.Empty))
        {
            Label1.Text = "<div>TODOS OS CAMPOS TÊM DE SER PREENCHIDOS</div>";
            e.Cancel = true;
        }   
        else 
            Label1.Text = "";

    }
    /*protected void IBRelatorio_Click(object sender, ImageClickEventArgs e)
    {
        string urlReportServer = ConfigurationManager.AppSettings["ReportServer"];
        RV.Visible = true;
        RV.ServerReport.ReportServerUrl = new Uri(urlReportServer); //Set the ReportServer Url
        RV.ServerReport.ReportPath = "/SIRCPME/RelatorioCEMEPLA"; //Passing the Report Path 



        List<ReportParameter> parameters = new List<ReportParameter>();
        parameters.Add(new ReportParameter("Ano", DDLAno.SelectedValue));

        if (DDLVersaoPME.SelectedValue.Equals("IN"))
        {
            parameters.Add(new ReportParameter("versaoPME", ""));
        }
        else
        {
            parameters.Add(new ReportParameter("versaoPME", "( " + DDLVersaoPME.SelectedItem.Text + " )"));
        }



        //RV.ServerReport.SetParameters(parameters);
        RV.ProcessingMode = ProcessingMode.Remote;
        RV.ShowParameterPrompts = false;
        RV.ShowPromptAreaButton = false;

        //* ********** gerar ficheiro / Mostra popup em pdf ********************
        Warning[] warnings;
        string[] streamids;
        string mimeType, encoding, extension; //deviceInfo;

        //deviceInfo = "True";

        byte[] bytes = RV.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

        Response.Buffer = true;
        Response.Clear();
        Response.ContentType = mimeType;


        //This header is for saving it as an Attachment and popup window should display to to offer save as or open a PDF file 
        Response.AddHeader("content-disposition", "attachment; filename=Relatorio." + extension);
        Response.BinaryWrite(bytes);

        Response.Flush();
        Response.End();

    }*/

    protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
    {
        
    }
}