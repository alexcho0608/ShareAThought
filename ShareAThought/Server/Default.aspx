<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Server._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<style>
    .content a{
        font-size:x-large;
    }
    .content a:hover{
        text-decoration:none;
    }
    .container .body-content{
        height:500px;
    }
</style>
    <div class="jumbotron text-center content" style="line-height:80px">
        <asp:HyperLink runat="server" NavigateUrl="/Topics" Width="800" Text="Topics"></asp:HyperLink>
        <br />
        <asp:HyperLink runat="server" NavigateUrl="Account/Register" Width="800" Text="Register"></asp:HyperLink>
        <br />
        <asp:HyperLink runat="server" NavigateUrl="Account/Login" Width="800" Text="Login"></asp:HyperLink>

    </div>

</asp:Content>
