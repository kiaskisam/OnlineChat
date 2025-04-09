<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateConversation.aspx.cs" Inherits="CreateConversation" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>创建会话</title>
    <!-- 引用外部的 CSS 文件 -->
    <link href="App_Themes/Css/StyleSheet_CreatConversation.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="createForm" runat="server">
        <div class="container">
            <h2>创建新会话</h2>

            <!-- 私聊选项 -->
            <div class="form-group">
                <label for="chkPrivateChat">是否为私聊</label>
                <asp:CheckBox ID="chkPrivateChat" runat="server" CssClass="form-control" OnCheckedChanged="chkPrivateChat_CheckedChanged" AutoPostBack="true" />
            </div>

            <!-- 会话头像 -->
            <div class="form-group">
                <label for="fileUploadAvatar">选择会话头像</label>
                <asp:FileUpload ID="fileUploadAvatar" runat="server" CssClass="form-control" />
            </div>

            <!-- 会话名称 -->
            <div class="form-group">
                <label for="txtConversationName">会话名称</label>
                <asp:TextBox ID="txtConversationName" runat="server" CssClass="form-control" placeholder="输入会话名称" />
            </div>

            <!-- 私聊对象 ID -->
            <div class="form-group private-id">
                <label for="txtPrivateId">私聊对象 ID</label>
                <asp:TextBox ID="txtPrivateId" runat="server" CssClass="form-control" placeholder="输入私聊对象的 ID" />
            </div>

            <!-- 最大人数 -->
            <div class="form-group max-participants">
                <label for="txtMaxParticipants">最大人数</label>
                <asp:TextBox ID="txtMaxParticipants" runat="server" CssClass="form-control" placeholder="输入最大人数" />
            </div>

            <!-- 错误提示 -->
            <div class="form-group">
                <asp:Label ID="error" runat="server"></asp:Label>
            </div>

            <!-- 创建会话按钮 -->
            <div class="form-group">
                <asp:Button ID="btnCreateConversation" runat="server" CssClass="btn-create" Text="创建会话" OnClick="btnCreateConversation_Click" />
            </div>

            <!-- 取消按钮 -->
            <div class="form-group">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn-cancel" Text="取消" OnClick="btnCancel_Click" />
            </div>
        </div>
    </form>

    <script>
        // 在私聊选项被选中时，显示私聊对象 ID 输入框，否则显示最大人数输入框
        function togglePrivateChatInputs() {
            var isPrivateChat = document.getElementById('<%= chkPrivateChat.ClientID %>').checked;
            var privateIdField = document.getElementsByClassName('private-id')[0];
            var maxParticipantsField = document.getElementsByClassName('max-participants')[0];
            var avatarField = document.getElementById('<%= fileUploadAvatar.ClientID %>'); // 获取上传文件组件

            if (isPrivateChat) {
                privateIdField.style.display = 'block'; // 显示私聊对象 ID
                maxParticipantsField.style.display = 'none'; // 隐藏最大人数
                avatarField.style.display = 'none'; // 隐藏上传头像
            } else {
                privateIdField.style.display = 'none'; // 隐藏私聊对象 ID
                maxParticipantsField.style.display = 'block'; // 显示最大人数
                avatarField.style.display = 'block'; // 显示上传头像
            }
        }

        // 调用 JavaScript 来切换输入框显示
        document.getElementById('<%= chkPrivateChat.ClientID %>').onclick = togglePrivateChatInputs;

        // 初始化状态
        window.onload = togglePrivateChatInputs;
    </script>
</body>
</html>