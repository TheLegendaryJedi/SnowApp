<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="True" Inherits="portfolio" Codebehind="portfolio.aspx.cs" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>Portfolio | Bootstrap .net Templates</title>
    <%-- ------ CSS ------ --%>
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="css/animate.min.css" rel="stylesheet" type="text/css" />
    <link href="css/prettyPhoto.css" rel="stylesheet" type="text/css" />
    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <link href="css/responsive.css" rel="stylesheet" type="text/css" />
     <link href="http://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css"
        rel="stylesheet" type="text/css" />
    <!--[if lt IE 9]>
    <script src="js/html5shiv.js"></script>
    <script src="js/respond.min.js"></script>
    <![endif]-->
    <link rel="shortcut icon" href="images/favicon.ico" />
    <style type="text/css">
        .auto-style1 {
            color: #FFFFFF;
            background-color: #000000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <header id="header">
    <%--<div class="top-bar">
    <div class="container">
    <div class="row">
    <div class="col-xs-6 col-sm-6 col-md-6 col-lg-6">
      <div class="top-number"><p><i class="fa fa-thumbs-up"></i> Keep In Tounch </p></div>
    </div>
     <div class="col-xs-6 col-sm-6 col-md-6 col-lg-6">
      <div class="social">
     <ul class="social-share">
        <li><a href="#"><i class="fa fa-facebook"></i></a></li>
        <li><a href="#"><i class="fa fa-twitter"></i></a></li>
        <li><a href="#"><i class="fa fa-linkedin"></i></a></li>        
        <li><a href="#"><i class="fa fa-skype"></i></a></li>
     </ul>     
       
    </div>
    </div>
    </div>
      </div>
        </div>  
        --%>
      <nav class="navbar navbar-inverse" role="banner">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar"></span>
                        <%--<span class="icon-bar"></span>
                        <span class="icon-bar"></span>--%>
                    </button>
                </div>
				
                <div class="collapse navbar-collapse navbar-right">
                    <ul class="nav navbar-nav">
                        <li><a href="Default.aspx">Home</a></li>
                       <li class="active"><a href="portfolio.aspx">Plan Winter</a></li>
                       <%--
                       <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown">Pages <i class="fa fa-angle-down"></i></a>
                            <ul class="dropdown-menu">
                                <li><a href="#">Blog Single</a></li>
                                <li><a href="#">Pricing</a></li>
                                <li><a href="#">404</a></li>
                                <li><a href="#">Shortcodes</a></li>
                            </ul>
                        </li>
                        
                        <li><a href="contactus.aspx">Contact</a></li>
                        --%>                      
                    </ul>
                </div>
            </div><!--/.container-->
        </nav><!--/nav-->
 
    </header>
    <section id="portfolio" class="no-margin">
    <div class="item active" style="background-image: url(images/slider/bg2.jpg)">
            <div class="container"> 
                <div class="center">
                    <br />
                    <h2 class="auto-style1">Start Planing Your Winter :)</h2>
                    <asp:FormView ID="FormView1" runat="server" BackColor="#276783" DataKeyNames="Nome" DataSourceID="SqlDataSource2" DefaultMode="Insert" Width="416px" OnItemInserted="FormView1_ItemInserted" OnItemInserting="FormView1_ItemInserting" ForeColor="White">
                        <EditItemTemplate>
                            Nome:
                            <asp:Label ID="NomeLabel1" runat="server" Text='<%# Eval("Nome") %>' />
                            <br />
                            Apelido:
                            <asp:TextBox ID="ApelidoTextBox" runat="server" Text='<%# Bind("Apelido") %>' />
                            <br />
                            Idade:
                            <asp:TextBox ID="IdadeTextBox" runat="server" Text='<%# Bind("Idade") %>' />
                            <br />
                            Sexo:
                            <asp:TextBox ID="SexoTextBox" runat="server" Text='<%# Bind("Sexo") %>' />
                            <br />
                            Disponibilidade:
                            <asp:TextBox ID="DisponibilidadeTextBox" runat="server" Text='<%# Bind("Disponibilidade") %>' />
                            <br />
                            <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                            &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            Nome:
                            <asp:TextBox ID="NomeTextBox" runat="server" Text='<%# Bind("Nome") %>' />
                            <br />
                            <br />
                            Apelido:
                            <asp:TextBox ID="ApelidoTextBox" runat="server" Text='<%# Bind("Apelido") %>' />
                            <br />
                            <br />
                            Idade:
                            <asp:TextBox ID="IdadeTextBox" runat="server" Text='<%# Bind("Idade") %>' />
                            <br />
                            <br />
                            Sexo:
                            <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("Sexo") %>'>
                                <asp:ListItem>Masculino</asp:ListItem>
                                <asp:ListItem>Feminino</asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            Disponibilidade:
                            <asp:Calendar ID="Calendar2" runat="server" Height="200px" Width="220px" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" SelectedDate='<%# Bind("Disponibilidade") %>'>
                                <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                                <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                                <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                                <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                                <WeekendDayStyle BackColor="#CCCCFF" />
                            </asp:Calendar>
                            <br />
                            Destino:&nbsp;<asp:DropDownList ID="DropDownList2" runat="server" SelectedValue='<%# Bind("NomeDestino") %>'>
                                <asp:ListItem>Andorra</asp:ListItem>
                                <asp:ListItem>Sierra Nevada</asp:ListItem>
                                <asp:ListItem>Béjar</asp:ListItem>
                                <asp:ListItem>Fuentes de Invierno</asp:ListItem>
                                <asp:ListItem>Baqueira-Beret</asp:ListItem>
                                <asp:ListItem>Chamonix</asp:ListItem>
                                <asp:ListItem>Val D&quot;Isère</asp:ListItem>
                                <asp:ListItem>Popova Sapka</asp:ListItem>
                                <asp:ListItem>Spindleruv Mlyn</asp:ListItem>
                                <asp:ListItem>Bansko</asp:ListItem>
                                <asp:ListItem>Cervinia</asp:ListItem>
                                <asp:ListItem>Jahorina </asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <br />
                            Outro Destino:&nbsp;<asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("OutroDestino") %>'></asp:TextBox>
                            <br />
                            <br />
                            <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" Text="Adicionar" BackColor="White" BorderColor="Black" BorderStyle="Solid" ForeColor="Black" />
                            &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancelar" BackColor="White" BorderColor="Black" BorderStyle="Solid" ForeColor="Black" />
                        </InsertItemTemplate>
                        <ItemTemplate>
                            Nome:
                            <asp:Label ID="NomeLabel" runat="server" BorderStyle="Solid" Text='<%# Eval("Nome") %>' />
                            <br />
                            Apelido:
                            <asp:Label ID="ApelidoLabel" runat="server" Text='<%# Bind("Apelido") %>' />
                            <br />

                            Idade:
                            <asp:Label ID="IdadeLabel" runat="server" Text='<%# Bind("Idade") %>' />
                            <br />
                            Sexo:
                            <asp:Label ID="SexoLabel" runat="server" Text='<%# Bind("Sexo") %>' />
                            <br />
                            Disponibilidade:
                            <asp:Label ID="DisponibilidadeLabel" runat="server" Text='<%# Bind("Disponibilidade") %>' />
                            <br />
                            <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                            &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
                            &nbsp;<asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                        </ItemTemplate>
                    </asp:FormView>
                    <br />
                    <br />
                    <asp:Label ID="Label1" runat="server" BackColor="Red" BorderColor="Yellow" BorderStyle="Solid" ForeColor="Black"></asp:Label>
                    <h2 class="auto-style1">Your Destinations so far:</h2>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Nome], [Idade], [Sexo], [Disponibilidade], [OutroDestino], [NomeDestino], [Apelido] FROM [Nomes]" ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [Nomes] WHERE [Nome] = @original_Nome AND [Idade] = @original_Idade AND [Sexo] = @original_Sexo AND [Disponibilidade] = @original_Disponibilidade AND (([OutroDestino] = @original_OutroDestino) OR ([OutroDestino] IS NULL AND @original_OutroDestino IS NULL)) AND [NomeDestino] = @original_NomeDestino AND [Apelido] = @original_Apelido" InsertCommand="INSERT INTO [Nomes] ([Nome], [Idade], [Sexo], [Disponibilidade], [OutroDestino], [NomeDestino], [Apelido]) VALUES (@Nome, @Idade, @Sexo, @Disponibilidade, @OutroDestino, @NomeDestino, @Apelido)" OldValuesParameterFormatString="original_{0}" UpdateCommand="UPDATE [Nomes] SET [Idade] = @Idade, [Sexo] = @Sexo, [Disponibilidade] = @Disponibilidade, [OutroDestino] = @OutroDestino, [NomeDestino] = @NomeDestino, [Apelido] = @Apelido WHERE [Nome] = @original_Nome AND [Idade] = @original_Idade AND [Sexo] = @original_Sexo AND [Disponibilidade] = @original_Disponibilidade AND (([OutroDestino] = @original_OutroDestino) OR ([OutroDestino] IS NULL AND @original_OutroDestino IS NULL)) AND [NomeDestino] = @original_NomeDestino AND [Apelido] = @original_Apelido">
                        <DeleteParameters>
                            <asp:Parameter Name="original_Nome" Type="String" />
                            <asp:Parameter Name="original_Idade" Type="String" />
                            <asp:Parameter Name="original_Sexo" Type="String" />
                            <asp:Parameter Name="original_Disponibilidade" Type="DateTime" />
                            <asp:Parameter Name="original_OutroDestino" Type="String" />
                            <asp:Parameter Name="original_NomeDestino" Type="String" />
                            <asp:Parameter Name="original_Apelido" Type="String" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Nome" Type="String" />
                            <asp:Parameter Name="Idade" Type="String" />
                            <asp:Parameter Name="Sexo" Type="String" />
                            <asp:Parameter Name="Disponibilidade" Type="DateTime" />
                            <asp:Parameter Name="OutroDestino" Type="String" />
                            <asp:Parameter Name="NomeDestino" Type="String" />
                            <asp:Parameter Name="Apelido" Type="String" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Idade" Type="String" />
                            <asp:Parameter Name="Sexo" Type="String" />
                            <asp:Parameter Name="Disponibilidade" Type="DateTime" />
                            <asp:Parameter Name="OutroDestino" Type="String" />
                            <asp:Parameter Name="NomeDestino" Type="String" />
                            <asp:Parameter Name="Apelido" Type="String" />
                            <asp:Parameter Name="original_Nome" Type="String" />
                            <asp:Parameter Name="original_Idade" Type="String" />
                            <asp:Parameter Name="original_Sexo" Type="String" />
                            <asp:Parameter Name="original_Disponibilidade" Type="DateTime" />
                            <asp:Parameter Name="original_OutroDestino" Type="String" />
                            <asp:Parameter Name="original_NomeDestino" Type="String" />
                            <asp:Parameter Name="original_Apelido" Type="String" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="Nome" DataSourceID="SqlDataSource2" AllowSorting="True" AutoGenerateSelectButton="True" HorizontalAlign="Left">
                            <Columns>
                                <asp:BoundField DataField="Nome" HeaderText="Nome" ReadOnly="True" SortExpression="Nome" />
                                <asp:BoundField DataField="Apelido" HeaderText="Apelido" SortExpression="Apelido" />
                                <asp:BoundField DataField="Idade" HeaderText="Idade" SortExpression="Idade" />
                                <asp:BoundField DataField="Sexo" HeaderText="Sexo" SortExpression="Sexo" />
                                <asp:BoundField DataField="Disponibilidade" HeaderText="Disponibilidade" SortExpression="Disponibilidade" />
                                <asp:BoundField DataField="NomeDestino" HeaderText="NomeDestino" SortExpression="NomeDestino" />
                                <asp:BoundField DataField="OutroDestino" HeaderText="OutroDestino" SortExpression="OutroDestino" />
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                        <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" DeleteCommand="DELETE FROM [Nomes] WHERE [Nome] = @Nome" InsertCommand="INSERT INTO [Nomes] ([Nome], [Apelido], [Idade], [Sexo], [Disponibilidade], [NomeDestino], [OutroDestino]) VALUES (@Nome, @Apelido, @Idade, @Sexo, @Disponibilidade, @NomeDestino, @OutroDestino)" SelectCommand="Select * from [Nomes] WHERE Nome = @Nome" UpdateCommand="UPDATE [Nomes] SET [Apelido] = @Apelido, [Idade] = @Idade, [Sexo] = @Sexo, [Disponibilidade] = @Disponibilidade, [NomeDestino] = @NomeDestino, [OutroDestino] = @OutroDestino WHERE [Nome] = @Nome">
                            <DeleteParameters>
                                <asp:Parameter Name="Nome" Type="String" />
                            </DeleteParameters>
                            <SelectParameters>
                                    <asp:ControlParameter Name="Nome" Type="String" ControlID="GridView1" PropertyName="SelectedValue" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="Nome" Type="String" />
                                <asp:Parameter Name="Apelido" Type="String" />
                                <asp:Parameter Name="Idade" Type="String" />
                                <asp:Parameter Name="Sexo" Type="String" />
                                <asp:Parameter Name="Disponibilidade" Type="DateTime" />
                                <asp:Parameter Name="NomeDestino" Type="String" />
                                <asp:Parameter Name="OutroDestino" Type="String" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Apelido" Type="String" />
                                <asp:Parameter Name="Idade" Type="String" />
                                <asp:Parameter Name="Sexo" Type="String" />
                                <asp:Parameter Name="Disponibilidade" Type="DateTime" />
                                <asp:Parameter Name="NomeDestino" Type="String" />
                                <asp:Parameter Name="OutroDestino" Type="String" />
                                <asp:Parameter Name="Nome" Type="String" />
                            </UpdateParameters>
                        </asp:SqlDataSource>
                        <br />
                        <asp:FormView ID="FormView2" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="Nome" DataSourceID="SqlDataSource3" GridLines="Both" OnItemDeleted="FormView2_ItemDeleted">
                            <EditItemTemplate>
                                Nome:
                                <asp:Label ID="NomeLabel1" runat="server" Text='<%# Eval("Nome") %>' />
                                <br />
                                Apelido:
                                <asp:TextBox ID="ApelidoTextBox" runat="server" Text='<%# Bind("Apelido") %>' />
                                <br />
                                Idade:
                                <asp:TextBox ID="IdadeTextBox" runat="server" Text='<%# Bind("Idade") %>' />
                                <br />
                                Sexo:
                                <asp:TextBox ID="SexoTextBox" runat="server" Text='<%# Bind("Sexo") %>' />
                                <br />
                                Disponibilidade:
                                <asp:TextBox ID="DisponibilidadeTextBox" runat="server" Text='<%# Bind("Disponibilidade") %>' />
                                <br />
                                NomeDestino:
                                <asp:TextBox ID="NomeDestinoTextBox" runat="server" Text='<%# Bind("NomeDestino") %>' />
                                <br />
                                OutroDestino:
                                <asp:TextBox ID="OutroDestinoTextBox" runat="server" Text='<%# Bind("OutroDestino") %>' />
                                <br />
                                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                                &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                            <EditRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <InsertItemTemplate>
                                Nome:
                                <asp:TextBox ID="NomeTextBox" runat="server" Text='<%# Bind("Nome") %>' />
                                <br />
                                Apelido:
                                <asp:TextBox ID="ApelidoTextBox" runat="server" Text='<%# Bind("Apelido") %>' />
                                <br />
                                Idade:
                                <asp:TextBox ID="IdadeTextBox" runat="server" Text='<%# Bind("Idade") %>' />
                                <br />
                                Sexo:
                                <asp:TextBox ID="SexoTextBox" runat="server" Text='<%# Bind("Sexo") %>' />
                                <br />
                                Disponibilidade:
                                <asp:TextBox ID="DisponibilidadeTextBox" runat="server" Text='<%# Bind("Disponibilidade") %>' />
                                <br />
                                NomeDestino:
                                <asp:TextBox ID="NomeDestinoTextBox" runat="server" Text='<%# Bind("NomeDestino") %>' />
                                <br />
                                OutroDestino:
                                <asp:TextBox ID="OutroDestinoTextBox" runat="server" Text='<%# Bind("OutroDestino") %>' />
                                <br />
                                <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
                                &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                            </InsertItemTemplate>
                            <ItemTemplate>
                                Nome:
                                <asp:Label ID="NomeLabel" runat="server" Text='<%# Eval("Nome") %>' />
                                <br />
                                Apelido:
                                <asp:Label ID="ApelidoLabel" runat="server" Text='<%# Bind("Apelido") %>' />
                                <br />
                                Idade:
                                <asp:Label ID="IdadeLabel" runat="server" Text='<%# Bind("Idade") %>' />
                                <br />
                                Sexo:
                                <asp:Label ID="SexoLabel" runat="server" Text='<%# Bind("Sexo") %>' />
                                <br />
                                Disponibilidade:
                                <asp:Label ID="DisponibilidadeLabel" runat="server" Text='<%# Bind("Disponibilidade") %>' />
                                <br />
                                NomeDestino:
                                <asp:Label ID="NomeDestinoLabel" runat="server" Text='<%# Bind("NomeDestino") %>' />
                                <br />
                                OutroDestino:
                                <asp:Label ID="OutroDestinoLabel" runat="server" Text='<%# Bind("OutroDestino") %>' />
                                <br />
                                <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                &nbsp;<asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
                                &nbsp;
                            </ItemTemplate>
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                        </asp:FormView>
                    <p>
                        &nbsp;</p>
                    <p>
                        <asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource4" Height="91px" Width="364px">
                            <Series>
                                <asp:Series Name="Series1" XValueMember="NomeDestino" YValueMembers="NomeDestino1">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1">
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </p>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT NomeDestino, COUNT(*) AS NomeDestino FROM Nomes GROUP BY NomeDestino"></asp:SqlDataSource>
                    <br />
                    <%-- <asp:ImageButton ID="IBRelatorio" runat="server" CssClass="btn btn-default" ImageUrl="~/images/pdf48.png" OnClick="IBRelatorio_Click" Height="70px" Width="67px" />
                    &nbsp;
                    <rsweb:ReportViewer ID="RV" runat="server" Height="700px" Visible="False" Width="1000px"></rsweb:ReportViewer>
                        --%>
                </div>
            </div>
    </div>
    </section>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            var offset = 300;
            var duration = 500;
            jQuery(window).scroll(function () {
                if (jQuery(this).scrollTop() > offset) {
                    jQuery('.back-to-top').fadeIn(duration);
                } else {
                    jQuery('.back-to-top').fadeOut(duration);
                }
            });

            jQuery('.back-to-top').click(function (event) {
                event.preventDefault();
                jQuery('html, body').animate({ scrollTop: 0 }, duration);
                return false;
            })
        });
    </script>
    <!-- /top-link-block -->
    <!-- Jscript -->
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/jquery.prettyPhoto.js" type="text/javascript"></script>
    <script src="js/jquery.isotope.min.js" type="text/javascript"></script>
    <script src="js/main.js" type="text/javascript"></script>
    <script src="js/wow.min.js" type="text/javascript"></script>
    </form>
</body>
</html>
