<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LikeControl.ascx.cs" Inherits="Server.Controls.LikeControl" %>

<asp:UpdatePanel runat="server" ID="ControlWrapper">
    <ContentTemplate>
        <div class="like-control col-md-1">
            <div class="likeMenu">
                <asp:Label runat="server" ID="LabelValue" CssClass="likeLabel" />
                <asp:LinkButton runat="server" ID="ButtonLike"  CssClass="btn btn-default glyphicon glyphicon-chevron-up" CommandArgument="<%# this.DataID %>" CommandName="Like" OnCommand="ButtonLike_Command" />
                <asp:LinkButton runat="server" ID="ButtonDislike" CssClass="btn btn-default glyphicon glyphicon-chevron-down" CommandArgument="<%# this.DataID %>" CommandName="Dislike" OnCommand="ButtonLike_Command" />
                            &nbsp Likes
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
