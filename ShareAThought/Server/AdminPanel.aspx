<%@ Page Title="AdminPanel" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="Server.AdminPanel" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="styles/admin.css" />
    <% if(isAdmin) { %>
    <div class="row">
        <h3>User search: 
        <asp:TextBox runat="server" Value="Search"
            ID="UserSearch" Width="300"></asp:TextBox>
        <asp:Button ID="SearchButtonControl"  runat="server" OnClick="Search"/>
        </h3>
        <p>
            Users List:<br />
            <asp:ListBox  CssClass="selectpicker userListControl" DataTextField="Text" runat="server" ID="ListUsersControl"></asp:ListBox>
        </p>
        <div>
            <asp:LinkButton runat="server" ID="DeleteUserButton" OnClientClick="return confirm('are you sure')" OnClick="DeleteUser" Text="Delete User" CssClass="btn btn-danger" />
            <asp:LinkButton runat="server" ID="LinkButton1" OnClientClick="return confirm('are you sure')" OnClick="PromoteUser" Text="Promote User" CssClass="btn btn-success" />

        </div>
    </div>
    <% } %>
    <%else{ %>
Access Denied!
    <% } %>
</asp:Content>
