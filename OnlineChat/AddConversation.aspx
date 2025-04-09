<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddConversation.aspx.cs" Inherits="ChatAdd " %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>查找会话</title>
    <link href="App_Themes/Css/StyleSheet_AddConversation.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="chatForm" runat="server">
        <div class="container">
            <h2>查找会话</h2>

            <!-- 查找框 -->
            <div class="search-box">
                <asp:TextBox ID="searchText" runat="server" CssClass="search-textbox" placeholder="输入会话名称或ID" />
                <asp:Button runat="server" Text="搜索" ID="btnSearch" CssClass="search-button" OnClick="btnSearch_Click" />
            </div>

            <!-- 查找结果 -->
            <div class="chat-list">
                <asp:Repeater ID="rptSearchResults" runat="server">
                    <ItemTemplate>
                        <div class="chat-item">
                            <asp:Image runat="server" 
                          ImageUrl='<%# Eval("Avatar") != DBNull.Value ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("Avatar")) : "Image/DefaultAvatar.jpg" %>' 
                          alt="会话头像" 
                          class="chat-avatar" Height="50" Width="50" />
                            <span><%# Eval("Title") %></span>
                            
                            <asp:Button ID="btnAddChat" runat="server" Text="添加" CommandArgument='<%# Eval("ConversationId") %>' CssClass="add-chat-button" OnClick="AddChat_Click" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="返回" />
        </p>
    </form>
</body>
</html>

