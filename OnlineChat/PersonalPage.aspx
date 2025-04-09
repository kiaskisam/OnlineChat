<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PersonalPage.aspx.cs" Inherits="PersonalPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

    <title>个人信息</title>
    <link href="App_Themes/Css/StyleSheet_PerspnalPage.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="personalForm" runat="server">
        <div class="container">
            <h2>个人信息</h2>
            <div class="profile-info">
                <asp:Image ID="imgAvatar" runat="server" CssClass="profile-avatar" />
                <label for="lblUserId">编号: <asp:Label ID="lblUserId" runat="server" /></label>
                <label for="lblUserName">姓名: <asp:Label ID="lblUserName" runat="server" /></label>
                <label for="lblGender">性别: <asp:Label ID="lblGender" runat="server" /></label>
                <label for="lblAge">年龄: <asp:Label ID="lblAge" runat="server" /></label>
            </div>
              <!-- 编辑表单部分（初始状态为隐藏） -->
            <div id="editForm" style="display:none;" >
                <div class="form-group" >
                    <label for="txtUserName">姓名:</label>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" />
                </div>
                <!-- FileUpload 控件用于上传头像 -->
                <div class="form-group">
                    <label for="fuAvatar">头像:</label>
                    <asp:FileUpload ID="fuAvatar" runat="server" Width="169px" Height="20px"/>
                </div>

                <div class="form-group"  >
                    <label for="ddlGender"> 性别:</label>
                    <asp:DropDownList ID="ddlGender" runat="server" Width="169px" Height="20px" >
                        <asp:ListItem Text="男" Value="男" />
                        <asp:ListItem Text="女" Value="女" />
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="txtAge">年龄:</label>
                    <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label for="txtAge">密码:</label>
                    <asp:TextBox ID="txtPsd" runat="server" CssClass="form-control" />
                </div>

                <asp:Button ID="btnSaveChanges" runat="server" Text="保存修改" CssClass="btn-save" OnClick="btnSaveChanges_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="btn-cancel" OnClientClick="cancelEdit(); return false;" />
            </div>
            <asp:Button ID="btnEdit" runat="server" Text="编辑个人信息" CssClass="btn-edit"/>

            &nbsp;<asp:Button ID="btnReturn" runat="server" Text="返回" CssClass="btn-return" OnClick="btnReturn_Click"/>
                    <!-- GridView 控件，初始状态为隐藏 -->
            <br />
            <asp:Label ID="lberror" runat="server"></asp:Label>
        </div>
       <script type="text/javascript">
           
           document.getElementById('btnEdit').onclick = function () {
               var editForm = document.getElementById('editForm');
               editForm.style.display = editForm.style.display === 'none' || editForm.style.display === '' ? 'block' : 'none';
               return false;
           };
           document.getElementById('btnCancel').onclick = function () {
               var editForm = document.getElementById('editForm');
               if (editForm.style.display = editForm.style.display === 'block' || editForm.style.display === '') editForm.style.display =' none';
               return false;
           };
                    </script>
    </form>
      </body>
</html>
