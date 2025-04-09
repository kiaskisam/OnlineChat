<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>登录</title>
    <link href="App_Themes/Css/StyleSheet_Login.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            font-weight: bold;
            height: 17px;
            width: 369px;
        }
        .auto-style3 {
            height: 17px;
        }
        .auto-style4 {
            font-weight: bold;
            height: 66px;
            width: 369px;
        }
        .auto-style5 {
            height: 66px;
            width: 439px;
        }
        .auto-style6 {
            font-weight: bold;
            width: 369px;
        }
        .auto-style7 {
            width: 369px;
        }
        .auto-style8 {
            width: 439px;
        }
        .auto-style9 {
            height: 17px;
            width: 439px;
        }
        .auto-style10 {
            height: 66px;
            width: 157px;
        }
        .auto-style11 {
            width: 157px;
        }
        .auto-style12 {
            height: 17px;
            width: 157px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="centered-div">
            <table>
                <tr>
                    <td class="auto-style4" style="text-align: right;">
                         </td>
                    <td class="auto-style5" style="text-align:center; ">
                        <asp:Image ID="AvatarImage" runat="server" Height="100px" Width="100px" ImageUrl="~/Image/DefaultAvatar.jpg" />

                    </td>
                    <td class="auto-style10" style="text-align:center; ">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4" style="text-align: right;">
                         &nbsp;</td>
                    <td class="auto-style5"style="text-align: center;">
                        <asp:Label ID="error" runat="server"></asp:Label>

                    </td>
                    <td class="auto-style10"style="text-align: center;">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style4" style="text-align: right;">
                         &nbsp;<asp:Label ID="Label1" runat="server" Text="用户名:" CssClass="label"></asp:Label>
                    </td>
                    <td class="auto-style5">
                        <asp:TextBox ID="lName" runat="server" CssClass="textbox"></asp:TextBox>
                    </td>
                    <td class="auto-style10">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style6" style="text-align: right;">
                        <asp:Label ID="Label2" runat="server" Text="密码:" CssClass="label"></asp:Label>
                        
                    </td>
                    <td class="auto-style8">
                       
                        <asp:TextBox ID="lPsd" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox>
                        
                    </td>
                    <td class="auto-style11">
                       
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    
                </tr>
                <tr>
                    <td class="auto-style1" style="text-align: right;">
                        <td class="auto-style9">
                        <asp:Button ID="btnSubmit" runat="server" Text="登录" CssClass="button" OnClick="btnSubmit_Click" />
                    </td>
                        <td class="auto-style12">
                        <a href="Register.aspx">注册</a>
                    </td>
                    <td class="auto-style3">
                        &nbsp;<td class="auto-style3">
                    </td>
                </tr>
                <tr>
                    <td class="auto-style7">&nbsp;</td>
                    <td class="auto-style8">
                        &nbsp;</td>
                    <td class="auto-style11">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style7">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style7" style="text-align: center">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Admin/Admin_Login.aspx">管理员入口</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

