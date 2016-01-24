<%@ Page Title="Topics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="Server.Topics" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <asp:ListView runat="server" ID="ListViewTopics" ItemType="Server.Models.Topic" DataKeyNames="ID" SelectMethod="ListViewTopics_GetData" InsertMethod="ListViewTopics_InsertItem" InsertItemPosition="LastItem">
        <LayoutTemplate>
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
            <div class="row">
                <h3><%#: Item.Title %></h3>
                <p>Category: <%#: Item.CategoryType %></p>
                <p>
                    <%#: Item.Author %>
                </p>
                <p><%#: Item.CreatedOn %></p>
            </div>
        </ItemTemplate>
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
</asp:Content>
