<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewTopic.aspx.cs" Inherits="Server.ViewTopic" %>

<%@ Register Src="~/Controls/LikeControl.ascx" TagPrefix="uc" TagName="LikeControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="styles/viewtopic.css" />
    <%if (User.Identity.GetUserId() != null) { %>
    <asp:FormView CssClass="formView" runat="server" ID="FormViewTopic" ItemType="Server.Models.Topic" SelectMethod="FormViewTopic_GetItem">
        <ItemTemplate>
            <h2 id="topicTitle"><%#: Item.Title %> </h2>
            <div id="topicContent">
                <div id="mainBlock">
                    <%#: Item.Content %>
                </div>
                <div class="row" id="contentFooter">
                    <div class="col-sm-4">
                        <p><%#: Item.CreatedOn %></p>
                    </div>
                    <div class="cols-sm-8">
                        <p class="text-right">Author:&nbsp <%#: Item.Author.UserName %> </p>
                    </div>
                </div>
            </div>
            </p>

            <uc:LikeControl runat="server" ID="LikeControl"
                Value="<%# GetLikes(Item) %>"
                CurrentUserVote="<%# GetCurrentUserVote(Item) %>"
                DataID="<%# Item.Id %>"
                OnLike="LikeControl_Like" />
        </ItemTemplate>
    </asp:FormView>
    <asp:ListView runat="server" ID="ListViewComments" ItemType="Server.Models.Comment"
        DataKeyNames="ID" SelectMethod="ListViewComments_GetData"
        InsertMethod="ListViewComments_InsertItem" UpdateMethod="ListViewComments_UpdateItem"
        InsertItemPosition="LastItem"
        OnItemDeleting="DeleteComment"
        OnItemDataBound="ListViewComments_ItemDataBound">
        <LayoutTemplate>
            <div runat="server" id="itemPlaceHolder"></div>
            <div class="row pager">
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
            <div class="row" id="commentContainer">
                <div class="row commentHeader">
                    <div class="col-sm-4">
                        <p>Created on: <%#: Item.CreatedOn %></p>
                    </div>
                    <div class="col-sm-8 text-right">
                        <p>Author: <%#: Item.Author.UserName %></p>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-1">
                        <img style="margin-top: 10%; margin-left: 4%" width="50px" height="50px" src="<%# getPath(Item.Author.UserName) %>" /></div>
                    <div class="col-md-11 commentText">
                        <div class="row" style="padding-bottom: 1%;">
                            <%#: Item.Content %>
                        </div>
                        <div class="row" style="padding-bottom: 4px; padding-left: 90%">
                                                    <asp:HiddenField runat="server" ID="IDValue" Value="<%# Item.Id %>" />
                            <asp:LinkButton Height="25px" Width="50px" Visible="<%# (Item.UserId == Context.User.Identity.GetUserId()) || isAdmin %>" runat="server" ID="LinkButton2" CssClass="btn btn-info " Text="Edit" CommandName="Edit" />
                            <asp:LinkButton Height="25px" Width="60px" Visible="<%# (Item.UserId == Context.User.Identity.GetUserId()) || isAdmin %>" runat="server" ID="LinkButton3" CssClass="btn btn-danger " Text="Delete" CommandName="Delete" />
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <EditItemTemplate>
            <div class="row">
                <p>
                    <asp:TextBox runat="server" ID="TextBoxEditContent" Text="<%# BindItem.Content %>" TextMode="MultiLine" Rows="6" Width="100%" /><asp:RequiredFieldValidator ErrorMessage="Content is required" ValidationGroup="EditContent" ControlToValidate="TextBoxEditContent" ForeColor="Red" runat="server" />
                </p>

                <asp:LinkButton runat="server" ID="ButtonEditComment" CssClass="btn btn-success" Text="Save" CommandName="Update" CausesValidation="true" ValidationGroup="EditComment" />
                <asp:LinkButton runat="server" ID="ButtonDeleteComment" CssClass="btn btn-danger" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                <div>
                    <i>by <%#: this.User.Identity.GetUserName() %></i>
                    <i>created on: <%# Item.CreatedOn %></i>
                </div>
            </div>
        </EditItemTemplate>
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
    <%} else { %>
    Access Denied!
    <%} %>
</asp:Content>
