<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Chatlist.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>在线聊天</title>
    <link href="App_Themes/Css/StyleSheet_ChatList.css" rel="stylesheet" type="text/css" />
    
</head>
<body>
     <form id="chatForm" runat="server">
        <div class="chat-container">
            <!-- 左侧聊天列表 -->
            <div class="chat-list">
                <h2>聊天列表</h2>
                <asp:Repeater ID="rptChatList" runat="server" >
                    <ItemTemplate>
                        <div class="chat-item">
                            <div class="chat-info">
                                
                                
                               <asp:HyperLink runat="server" NavigateUrl='<%# "ConversationDetails.aspx?ConversationId=" + Eval("ConversationId") %>' >
                                    <asp:Image runat="server" 
                                               ImageUrl='<%# Eval("ConversationAvatar") != DBNull.Value ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("ConversationAvatar")) : "Image/DefaultAvatar.jpg" %>' 
                                               alt="会话头像" 
                                               class="chat-avatar" />
                                </asp:HyperLink>
                                <!-- 按钮用于捕获点击事件 -->
                                <asp:LinkButton ID="btnSelectConversation"  Font-Underline="false"
                                runat="server" 
                                CommandName="SelectConversation" 
                                CommandArgument='<%# Eval("ConversationId") + "," + Eval("ConversationName") %>' 
                                Onclick="btnSelectConversation_Click"
                                > 
                                 <!-- 会话名称 -->
                                <span class="user-name"><%# Eval("ConversationName") %></span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                  <!-- 添加聊天按钮 -->
                 <asp:Button runat="server" type="button" class="add-chat-button" Text="添加会话" ID="baddchat" />

                <!-- 下拉框（显示“创建”和“加入”选项） -->
                <div id="chatDropdown" class="dropdown">
                    <asp:DropDownList ID="ddlChatAction" runat="server" OnSelectedIndexChanged="ddlChatAction_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="" Text="请选择操作" />
                        <asp:ListItem Value="create" Text="创建会话" />
                        <asp:ListItem Value="join" Text="加入会话" />
                    </asp:DropDownList>
                    
                </div>
            </div>
           


            <!-- 右侧聊天窗口 -->
            <div class="chat-window">
                <div id="chatHeader">
                    <span id="chatTitle"><%= ConversationName %></span>
                </div>
                <div id="chatMessages" class="chat-messages">
                    <!-- 用来绑定消息的Repeater -->
                    <asp:Repeater ID="rptChatMessages" runat="server" >
                        <ItemTemplate>
                              <div class="message <%# Eval("SenderId") != DBNull.Value && Eval("SenderId") != null && Convert.ToInt32(Eval("SenderId")) == (user != null ? user.Id : 0) ? "sent" : "received" %>">
            
                            <!-- 显示发送者头像 -->
                            <asp:Image runat="server" 
                                       ImageUrl='<%# Eval("SenderAvatar") != DBNull.Value ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("SenderAvatar")) : "Image/DefaultAvatar.jpg" %>' 
                                       alt="发送者头像" 
                                       Height="50" Width="50" />
            
                            <!-- 显示发送者名字 -->
                            <span class="message-sender"><%# Eval("SenderName") %></span>

                            <!-- 显示消息内容 -->
                            <div class="message-content"><%# Eval("Content") %></div>

                            <!-- 显示消息时间 -->
                            <div class="message-timestamp"><%# Eval("Timestamp", "{0:HH:mm}") %></div>
                        </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="chat-input">
                    <asp:TextBox ID="txtMessage" runat="server" CssClass="chat-input-textbox" Placeholder="输入消息..." />
                    <asp:Button ID="btnSend" runat="server" Text="发送" CssClass="chat-send-button" OnClick="btnSend_Click" />
                </div>
            </div>
           <!-- 个人头像框（左下角） -->
         <div class="profile-picture">
            <asp:Image runat="server" ImageUrl="Image/DefaultAvatar.jpg" alt="个人头像" id="profilePic" onclick="window.location.href='PersonalPage.aspx';" />
             
           
        </div>

        </div>
    <script>
        // 控制下拉框显示和隐藏
        document.getElementById('baddchat').onclick = function () {
            var dropdown = document.getElementById('chatDropdown');
            dropdown.style.display = dropdown.style.display === 'none' || dropdown.style.display === '' ? 'block' : 'none';
            return false;
        };
        setInterval(function () {
            var conversationId = '<%= Session["ConversationId"] %>'; // 获取会话 ID
             $.ajax({
                 type: "GET",
                 url: "GetMessages.aspx", // 请求获取聊天消息的后台页面
                 data: { conversationId: conversationId },
                 success: function (data) {
                     // 更新聊天消息
                     $('#chatMessages').html(data); // 更新聊天消息区域的 HTML
                 }
             });
         }, 3000); // 每 3 秒请求一次
                                </script>
        
    </form>
</body>
</html>