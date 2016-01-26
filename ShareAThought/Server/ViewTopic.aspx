<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewTopic.aspx.cs" Inherits="Server.ViewTopic" %>

<%@ Register Src="~/Controls/LikeControl.ascx" TagPrefix="uc" TagName="LikeControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView runat="server" ID="FormViewTopic" ItemType="Server.Models.Topic" SelectMethod="FormViewTopic_GetItem">
        <ItemTemplate>
            <h2><%#: Item.Title %> </h2>
            <p>Category: <%#: Item.CategoryType %></p>
            <p><%#: Item.Content %></p>
            <p>
                Author: <%#: Item.Author.UserName %>
            </p>
            <p><%#: Item.CreatedOn %></p>

            <uc:LikeControl runat="server" ID="LikeControl"
                Value="<%# GetLikes(Item) %>"
                CurrentUserVote="<%# GetCurrentUserVote(Item) %>"
                DataID="<%# Item.Id %>"
                OnLike="LikeControl_Like" />
        </ItemTemplate>
    </asp:FormView>

</asp:Content>
