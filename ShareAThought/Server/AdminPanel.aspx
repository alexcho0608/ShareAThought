<%@ Page Title="AdminPanel" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="Server.AdminPanel" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="styles/admin.css" />
    <% if(isAdmin) { %>
    <div class="row">
        <h3>User search: 
        <asp:TextBox runat="server" Value="Search"
            ID="UserSearch" Width="300"></asp:TextBox>
        <asp:Button ID="SearchButtonControl"  runat="server" OnClick="Search" Text="Search"/>
        </h3>
        <p>
            <b> Users List: </b><br />
            <asp:ListBox Height="500px" CssClass="selectpicker userListControl" DataTextField="Text" runat="server" ID="ListUsersControl"></asp:ListBox>
        </p>
        <div>
            <asp:LinkButton runat="server" ID="DeleteUserButton" OnClientClick="return confirm('are you sure')" OnClick="DeleteUser" Text="Suspend User" CssClass="btn btn-danger" />
            <asp:LinkButton runat="server" ID="LinkButton1" OnClientClick="return confirm('are you sure')" OnClick="PromoteUser" Text="Promote User" CssClass="btn btn-success" />
            <asp:LinkButton runat="server" ID="LinkButton2" OnClick="UnsuspendUser" Text="Unsuspend" CssClass="btn btn-success"></asp:LinkButton>
        </div>
    </div>
    <% } %>
    <%else{ %>
Access Denied!
    <% } %>
</asp:Content>
