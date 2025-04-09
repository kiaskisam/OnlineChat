<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConversationDetails.aspx.cs" Inherits="ConversationDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>会话详情</title>
    <link href="App_Themes/Css/StyleSheet_ConversationDetails.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="conversationForm" runat="server">
        <div class="container">
            <!-- 会话基本信息 -->
            <div class="conversation-info">
                <asp:Image ID="imgConversationAvatar" runat="server" CssClass="conversation-avatar" />
                <br />
                <label for="conversationId">会话ID: <asp:Label ID="lblConversationId" runat="server" /></label>
                <br />
                <label for="conversationName">会话名称: <asp:Label ID="lblConversationName" runat="server" /></label>
                <br />
                <label for="creator">创建者: <asp:Label ID="lblCreator" runat="server" /></label>
                <br />
                <!-- 编辑按钮 -->
              <asp:Button ID="btnEditConversation" runat="server" Text="编辑会话" CssClass="btn-edit"  />

            <!-- 编辑表单部分（初始状态为隐藏） -->
            <div id="editForm" style="display:none;">
                <div class="form-group">
                    <label for="txtConversationName">会话名称:</label>
                    <asp:TextBox ID="txtConversationName" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="fuAvatar">选择头像:</label>
                    <asp:FileUpload ID="fuAvatar" runat="server" />
                </div>

                <div class="form-group">
                    <asp:Button ID="btnSaveChanges" runat="server" Text="保存修改" CssClass="btn-save" OnClick="btnSaveChanges_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="btn-cancel" OnClientClick="cancelEdit(); return false;" />
                </div>
            </div>

            <!-- 会话成员 -->
            <div class="conversation-members">
                <h3>会话成员</h3>
                <asp:Repeater ID="rptMembers" runat="server">
                    <ItemTemplate>
                        <div class="member-item">
                            <asp:Image runat="server" ImageUrl='<%# Eval("Avatar") != DBNull.Value ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("Avatar")) : "Image/DefaultAvatar.jpg" %>' alt="成员头像" class="member-avatar" />
                            <span class="member-name"><%# Eval("Name") %></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- 错误提示 -->
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>

            <!-- 返回按钮 -->
            <asp:Button ID="btnReturn" runat="server" Text="返回" CssClass="btn-return" OnClick="btnReturn_Click" />
        </div>
    </form>

    <script type="text/javascript">
        // 页面加载时，设置会话信息
        document.addEventListener('DOMContentLoaded', function () {
            var conversationId = '<%= Request.QueryString["ConversationId"] %>';

            // 使用 AJAX 获取会话详细信息
            fetch('GetConversationDetails.aspx?ConversationId=' + conversationId)
                .then(response => response.json())
                .then(data => {
                    document.getElementById('conversationName').textContent = data.ConversationName;
                    document.getElementById('lblConversationId').textContent = data.ConversationId;
                    document.getElementById('lblCreator').textContent = data.CreatorName;
                    document.getElementById('imgConversationAvatar').src = 'data:image/jpeg;base64,' + data.ConversationAvatar;
                });
        });

        // 显示/隐藏编辑表单
        document.getElementById('btnEditConversation').onclick = function () {
            var editForm = document.getElementById('editForm');
           /* var conversationName = document.getElementById('conversationName').textContent;
            document.getElementById('txtConversationName').value = conversationName;*/ // 填充会话名称

            // 切换表单显示状态
            editForm.style.display = editForm.style.display === 'none' || editForm.style.display === '' ? 'block' : 'none';
            return false;
        }


        // 取消编辑
        function cancelEdit() {
            document.getElementById('editForm').style.display = 'none';
        }
    </script>
</body>
</html>