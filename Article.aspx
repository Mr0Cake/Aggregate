<%@ Page Title="" Language="C#" MasterPageFile="~/AnonPage.master" AutoEventWireup="true" CodeFile="Article.aspx.cs" Inherits="Article" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Stylesheets" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" Runat="Server">
    <section>
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="ObjectDataSource1">
        <%--<asp:DataPager runat="server"></asp:DataPager>--%>
    </asp:Repeater>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetFeedItemByID" TypeName="BLL.FeedItemBLL">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="1" Name="ID" QueryStringField="ID" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </section>
</asp:Content>

