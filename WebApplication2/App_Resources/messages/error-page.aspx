<%@ Page Language="C#" MasterPageFile="~/App_Resources/default.master" AutoEventWireup="true"
    Inherits="Public_Error_Page" Title="Erro na Aplicação" CodeBehind="error-page.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContentPlaceholder" runat="Server">
    <h1 class="title-regular clearfix">
        NManif - Erro na Aplicação</h1>
    <p class="alpha">Desde já pedimos desculpas pelo erro. Por favor contacte o apoio ao 
        utilizador para reportar o mesmo.</p>
    <p class="error">
        Contactos do apoio ao utilizador:</p>
    <p class="green" style="text-align: center">
        Repartição de Comunicações e Sistemas de Informação</p>
    <p class="gray">
        -&gt; Telefone: 422521</p>
    <p class="gray">
        -&gt; Email: alves.jc@mail.exercito.pt</p>
    <p style="text-align: center">
        <asp:Image ID="Image1" runat="server" 
            ImageUrl="~/App_Resources/images/dcsi.png" Height="300px" Width="201px" />
    </p>
    <p>
        &nbsp;</p>
</asp:Content>
