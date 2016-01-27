<%@ Page Title="Topics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="Server.Topics" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="topicsContainer">
    <link rel="stylesheet" href="styles/topics.css" />
     <h2><%: Title %>.</h2>
    <asp:ListView runat="server" ID="ListViewTopics" ItemType="Server.Models.Topic" DataKeyNames="ID" SelectMethod="ListViewTopics_GetData" InsertMethod="ListViewTopics_InsertItem" InsertItemPosition="LastItem" UpdateMethod="ListViewTopics_UpdateItem" OnSorting="ListViewTopics_Sorting">
        <LayoutTemplate>
            <div class="row searchPanel" >
                <asp:LinkButton runat="server" ID="ButtonSortByTitle" CssClass="col-md-2 btn btn-default" Text="Sort by Title" CommandArgument="Title" CommandName="Sort" />
                <asp:LinkButton runat="server" ID="ButtonSortByDate" CssClass="col-md-2 btn btn-default" Text="Sort by Date" CommandArgument="CreatedOn" CommandName="Sort" />
                <asp:LinkButton runat="server" ID="ButtonSortByCategory" CssClass="col-md-2 btn btn-default" Text="Sort by Category" CommandArgument="CategoryType" CommandName="Sort" />
            </div>
            <div runat="server" id="itemPlaceHolder"></div>
            <div class="row">
                <asp:DataPager runat="server" ID="DataPagerTopics" PagedControlID="ListViewTopics" PageSize="5">
                    <Fields>
                        <asp:NextPreviousPagerField ShowNextPageButton="false" ShowPreviousPageButton="true" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField ShowNextPageButton="true" ShowPreviousPageButton="false" />
                    </Fields>
                </asp:DataPager>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div class="row topicHeader">
                <h3><asp:hyperlink navigateurl='<%# "~/ViewTopic?id=" + Item.Id %>' runat="server" Text="<%#: Item.Title %>" />
                </h3>
             </div>
            <div class="row topicInfo"> 
               <div class="col-sm-4"><p>Category: <%#: Item.CategoryType %></p></div>
                <div class="col-sm-4"><p>Author: <%#: Item.Author.UserName %></p></div>
                <div class="col-sm-4"><p>Created on: <%#: Item.CreatedOn %></p></div>
            </div>
            <div class="row topicButtons">
                <asp:LinkButton Visible="<%# (Item.UserId == Context.User.Identity.GetUserId()) || isAdmin %>" runat="server" ID="ButtonEditTopic" CssClass="btn btn-info " Text="Edit" CommandName="Edit" />
                <asp:LinkButton Visible="<%# (Item.UserId == Context.User.Identity.GetUserId()) || isAdmin %>" runat="server" ID="DeleteTopic" CssClass="btn btn-danger " Text="Delete" OnClick="DeleteTopic" />
            </div>
            <hr />
        </ItemTemplate>
        <EditItemTemplate>
            <div class="row">
                <h3>
                    <asp:TextBox runat="server" ID="TextBoxEditTitle" Text="<%# BindItem.Title %>" />
                    <asp:RequiredFieldValidator ErrorMessage="Title is required" ValidationGroup="EditTopic" ControlToValidate="TextBoxEditTitle" ForeColor="Red" runat="server" />
                    <asp:LinkButton runat="server" ID="ButtonEditTopic" CssClass="btn btn-success" Text="Save" CommandName="Update" CausesValidation="true" ValidationGroup="EditTopic" />
                    <asp:LinkButton runat="server" ID="ButtonDeleteTopic" CssClass="btn btn-danger" Text="Cancel" CommandName="Cancel" CausesValidation="false" />
                </h3>
                <p>
                    <asp:DropDownList runat="server" ID="DropDownListCategories"
                        SelectedValue="<%#: BindItem.CategoryType %>" SelectMethod="DropDownListCategories_GetData" />
                <p>
                    <asp:TextBox runat="server" ID="TextBoxEditContent" Text="<%# BindItem.Content %>" TextMode="MultiLine" Rows="6" Width="100%" /><asp:RequiredFieldValidator ErrorMessage="Content is required" ValidationGroup="EditTitle" ControlToValidate="TextBoxEditContent" ForeColor="Red" runat="server" />
                </p>
                <div>
                    <i>by <%#: this.User.Identity.GetUserName() %></i>
                    <i>created on: <%# Item.CreatedOn %></i>
                </div>
            </div>
        </EditItemTemplate>
        <InsertItemTemplate>
            <a href="#" id="buttonShowInsertPanel" class="btn btn-info pull-right" onclick="(function (ev) { $('#panelInsertTopic').show(); $('#buttonShowInsertPanel').hide(); }())">Insert new Topic</a>
            <div id="panelInsertTopic" style="display: none;">

                <div class="row">
                    <h3>Title: 
                    <asp:TextBox runat="server" ID="TextBoxInsertTitle" Width="300" Text="<%#: BindItem.Title %>"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Title is required!" ValidationGroup="InsertTopic" ControlToValidate="TextBoxInsertTitle" ForeColor="Red" runat="server" />
                    </h3>
                    <p>
                        Category: 
                    <asp:DropDownList runat="server" ID="DropDownListCategories" ItemType="Server.Models.Category" SelectedValue="<%#: BindItem.CategoryType %>" SelectMethod="DropDownListCategories_GetData">
                    </asp:DropDownList>
                    </p>
                    <p>
                        Content: 
                    <asp:TextBox runat="server" ID="TextBoxInsertContent" Width="300" TextMode="MultiLine" Text="<%#: BindItem.Content %>" Rows="6"></asp:TextBox>
                        <asp:RequiredFieldValidator ErrorMessage="Content is Required!" ValidationGroup="InsertTopic" ControlToValidate="TextBoxInsertContent" ForeColor="Red" runat="server" />
                    </p>
                    <div>
                        <asp:LinkButton runat="server" ID="ButtonInsertTopic" CssClass="btn btn-success" CommandName="Insert" Text="Insert" CausesValidation="true" ValidationGroup="InsertTopic" />
                        <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-danger" CommandName="Cancel" Text="Cancel" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </InsertItemTemplate>
    </asp:ListView>
   </div>
</asp:Content>
