<%@ Page Title="" Language="C#" MasterPageFile="~/AnonPage.master" AutoEventWireup="true" CodeFile="NewsFeed.aspx.cs" Inherits="NewsFeed" %>

<asp:Content ID="Content2" ContentPlaceHolderID="StyleSheets" Runat="Server">
    <link href="App_Themes/FeedItems.css" rel="stylesheet"/>
    <script type="text/javascript">
        $(function () {
            var bigItemChecked = false;
            var randomint = Math.floor((Math.random() * 3));
            $('.ArticleContainer').each(function (i, obj) {
                if (i % 3 == 0 || i == 0) {
                    bigItemChecked = false;
                    randomint = Math.floor((Math.random() * 3));
                }
                if (!bigItemChecked && (i+1)%3 == randomint) {
                    obj.className += " big";
                    bigItemChecked = true;
                }

            });


        });
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
    
    <asp:ListView ID="ListView1" runat="server" DataSourceID="FeedItemsDS">
        <LayoutTemplate>
            <section >
              <article runat="server" id="itemPlaceholder" ></article>
                <asp:DataPager ID="DataPager1" runat="server" PageSize="9">
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="False" />
                    </Fields>
                </asp:DataPager>
            </section>
        </LayoutTemplate>
       
        <EmptyDataTemplate>
            <span>No data was returned.</span>
        </EmptyDataTemplate>
        
        <ItemTemplate>
            <article class="ArticleContainer">
            <div class="image">
	            <%# Eval("ImageLink") %>
            </div>
            <asp:HyperLink class="ItemTitle" ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("ArticleLink") %>'><%# Eval("Title") %></asp:HyperLink>
            <footer>
                <p class="ItemDate"><%# Eval("PublishDate") %></p>
            </footer>
            </article>
        </ItemTemplate>
        
        <SelectedItemTemplate>
            <article class="ItemPlaceholderContainer">
            <div class="image">
	            <%# Eval("ImageLink") %>
            </div>
            <asp:HyperLink class="ItemTitle" ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("ArticleLink") %>'><%# Eval("Title") %></asp:HyperLink>
            <footer>
                <p class="ItemDate"><%# Eval("PublishDate") %></p>
            </footer>
            </article>
        </SelectedItemTemplate>
    </asp:ListView>

    <asp:ObjectDataSource ID="FeedItemsDS" runat="server" SelectMethod="GetAllFeedItems" TypeName="BLL.FeedItemBLL"></asp:ObjectDataSource>

</asp:Content>

