<%@ Page Title="Página de Logout" Language="C#" MasterPageFile="~/App_Resources/default.master"
    AutoEventWireup="true" Inherits="secured_log_in_log_out" Codebehind="log-out.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <h1 class="title-regular clearfix">
        Logout efectuado com sucesso</h1>
    <p align="center">
        <asp:Image ID="Image2" runat="server" ImageAlign="Middle" 
            ImageUrl="~/App_Resources/images/dcsi.png" EnableTheming="True" Width="300px" />
    </p>
</asp:Content>
