<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>注册</title>
     <link href="App_Themes/Css/StyleSheet_Register.css" rel="stylesheet" type="text/css" />
</head>
<body>
     <form id="form1" runat="server" enctype="multipart/form-data">
        
        <div class="form-group">
            <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName">用户名:</asp:Label>
            <asp:TextBox ID="txtName" runat="server" AutoPostBack="True"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" ErrorMessage="请输入用户名"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword">密码:</asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" AutoPostBack="False"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ErrorMessage="请输入密码"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <asp:Label ID="lblAge" runat="server" AssociatedControlID="txtAge">年龄:</asp:Label>
            <asp:TextBox ID="txtAge" runat="server"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtAge" ErrorMessage="请输入合理的年龄" MaximumValue="100" MinimumValue="0" Type="Integer"></asp:RangeValidator>
        </div>
        <div class="form-group">
            <asp:Label ID="lblGender" runat="server" AssociatedControlID="ddlGender">性别:</asp:Label>
            <asp:DropDownList ID="ddlGender" runat="server">
                <asp:ListItem Value="">请选择性别</asp:ListItem>
                <asp:ListItem Value="男">男</asp:ListItem>
                <asp:ListItem Value="女">女</asp:ListItem>
                <asp:ListItem Value="other">其他</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="lblAvatar" runat="server" AssociatedControlID="fuAvatar">头像:</asp:Label>
            <asp:FileUpload ID="fuAvatar" runat="server" />
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtAge">输入验证码:</asp:Label>
            <asp:TextBox ID="tCaptcha" runat="server"></asp:TextBox>
            
            <asp:Image ID="ImgCap" runat="server" Height="50px" Width="100px" />
            <asp:Label ID="error" runat="server"></asp:Label>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tCaptcha" ErrorMessage="请输入验证码"></asp:RequiredFieldValidator>
            
        </div>
        </div>
        <div class="form-group" id="centered-div">
            &nbsp;<asp:Button ID="btnRegister" runat="server" Text="注册" OnClick="btnRegister_Click" Width="159px" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <a href="Login.aspx">返回</a>
        </div>
         
    </form>
</body>
</html>
