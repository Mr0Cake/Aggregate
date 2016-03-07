<%@ Page Title="" Language="C#" MasterPageFile="~/AnonPage.master" AutoEventWireup="true" CodeFile="Categories.aspx.cs" Inherits="Categories" %>


<asp:Content ID="Content2" ContentPlaceHolderID="StyleSheets" Runat="Server">
    <%--<link href="App_Themes/FeedItems.css" rel="stylesheet"/>--%>
    <link href="App_Themes/Categories.css" rel="stylesheet"/>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
    <section>
        <asp:ListView ID="ListView1" runat="server" DataSourceID="FeedDS">
            
            
            <EmptyDataTemplate>
                <span>No data was returned.</span>
            </EmptyDataTemplate>
            
            <ItemTemplate>
                
                <asp:Label ID="HomePageLabel" runat="server" Text='<%# Eval("HomePage") %>' />
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("HomePage") %>'><%# Eval("HomePage") %></asp:HyperLink>
                    <asp:Repeater ID="Repeater1" runat="server" DataSource='<%# Eval("Top3")%>'>
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>

                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='~/Article.aspx?ID=<%# Eval("ID") %>'><%# Eval("Title") %></asp:HyperLink>
                        </li>
                    </ItemTemplate>

                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>

                </asp:Repeater>
                
            </ItemTemplate>
            <LayoutTemplate>
                <article id="itemPlaceholderContainer" runat="server" style="">
                    <span runat="server" id="itemPlaceholder" />
                </article>
            </LayoutTemplate>
            
        </asp:ListView>
        
    </section>
    <asp:ObjectDataSource ID="FeedDS" runat="server" SelectMethod="GetAllFeeds" TypeName="BLL.FeedBLL"></asp:ObjectDataSource>

    
</asp:Content>

