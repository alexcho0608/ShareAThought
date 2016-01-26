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
    <asp:ListView runat="server" ID="ListViewComments" ItemType="Server.Models.Comment" DataKeyNames="ID" SelectMethod="ListViewComments_GetData" InsertMethod="ListViewComments_InsertItem" InsertItemPosition="LastItem">
        <LayoutTemplate>
            <div runat="server" id="itemPlaceHolder"></div>
            <div class="row">
                <asp:DataPager runat="server" ID="DataPagerComments" PagedControlID="ListViewComments" PageSize="10">
                    <Fields>
                        <asp:NextPreviousPagerField ShowNextPageButton="false" ShowPreviousPageButton="true" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ShowNextPageButton="true" ShowPreviousPageButton="false" />
                    </Fields>
                </asp:DataPager>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div class="row">
                <p><%#: Item.CreatedOn %></p>
                <p><%#: Item.Author.UserName %>" </p>
                <p><%#: Item.Content %></p>
            </div>
        </ItemTemplate>
        <InsertItemTemplate>
            <a href="#" id="buttonShowInsertPanel" class="btn btn-info pull-right" onclick="(function (ev) { $('#panelInsertComment').show(); $('#buttonShowInsertPanel').hide(); }())">Add new Comment</a>
            <div id="panelInsertComment" style="display: none;">
                <div class="row">
                    <p>
                        Content: 
                    <asp:TextBox runat="server" ID="TextBoxInsertContent" Width="300" TextMode="MultiLine" Text="<%#: BindItem.Content %>" Rows="6"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Content is Required!" ValidationGroup="InsertComment" ControlToValidate="TextBoxInsertContent" ForeColor="Red" runat="server" />
                    </p>
                    <div>
                        <asp:LinkButton runat="server" ID="ButtonInsertComment" CssClass="btn btn-success" CommandName="Insert" Text="Insert" CausesValidation="true" ValidationGroup="InsertComment" />
                        <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-danger" CommandName="Cancel" Text="Cancel" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </InsertItemTemplate>
    </asp:ListView>

</asp:Content>
