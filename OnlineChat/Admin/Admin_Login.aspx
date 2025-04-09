<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin_Login.aspx.cs" Inherits="Admin_Admin_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>管理员登录</title>
    <link href="../App_Themes/Css/StyleSheet_AdminLogin.css" rel="stylesheet" type="text/css" /><!-- 记得返回上一级目录 -->
</head>
<body>
    <form id="loginForm" runat="server">
        <div class="login-container">
            <h2>管理员登录</h2>

            <!-- 用户名 -->
            <div class="form-group">
                <label for="txtUsername">用户名:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="required"></asp:TextBox>
            </div>

            <!-- 密码 -->
            <div class="form-group">
                <label for="txtPassword">密码:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" required="required"></asp:TextBox>
            </div>
            <div class="form-group">
             <label for="txtUsername">验证码:</label>
             <asp:TextBox ID="txtcap" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                <asp:Image ID="Imgcap" runat="server" Height="50px" Width="80px" />
         </div>

            <!-- 登录按钮 -->
            <div class="form-group">
                <asp:Button ID="btnLogin" runat="server" Text="登录" CssClass="btn-login" OnClick="btnLogin_Click" />
            </div>

            <!-- 错误提示 -->
            <asp:Label ID="lblError" runat="server" ForeColor="Red" />
        </div>
    </form>
</body>
</html>
