<%@ Page Title="AdminPanel" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="AdminPanel.aspx.cs" Inherits="Server.AdminPanel" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <% if(isAdmin) { %>
    <div class="row">
        <h3>User search: 
        <asp:TextBox runat="server" AutoPostBack="True" Value="Search" OnTextChanged="TextBox2_TextChanged"
            ID="UserSearch" Width="300"></asp:TextBox>
        </h3>
        <p>
            Users List:<br />
            <asp:ListBox DataTextField="Text" runat="server" ID="ListUsersId"></asp:ListBox>
        </p>
        <div>
            <asp:LinkButton runat="server" ID="DeleteUserButton" OnClientClick="return confirm('are you sure')" OnClick="PromoteUser" Text="Delete User" CssClass="btn btn-danger" />
            <asp:LinkButton runat="server" ID="LinkButton1" OnClientClick="return confirm('are you sure')" OnClick="PromoteUser" Text="PromoteUser" CssClass="btn btn-success" />

        </div>
    </div>
    <% } %>
    <%else{ %>
Access Denied!
    <% } %>
</asp:Content>
