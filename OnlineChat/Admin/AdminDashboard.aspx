<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminDashboard.aspx.cs" Inherits="Admin_AdminDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Admin Dashboard</title>
    <link href="../App_Themes/Css/StyleSheet_AdminDashboard.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            margin-right: 1px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- 使用 flex 布局包裹两个 GridView -->
            <div class="grid-container">
                <!-- 第一个 GridView -->
                <div class="grid-view">
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource_User">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                            <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                            <asp:BoundField DataField="Age" HeaderText="Age" SortExpression="Age" />
                            <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource_User" runat="server" ConnectionString="<%$ ConnectionStrings:OnlineChatConnectionString %>" DeleteCommand="DELETE FROM [Users] WHERE [ID] = @ID" InsertCommand="INSERT INTO [Users] ([Name], [Password], [Age], [Gender], [Avatar]) VALUES (@Name, @Password, @Age, @Gender, @Avatar)" SelectCommand="SELECT * FROM [Users]" UpdateCommand="UPDATE [Users] SET [Name] = @Name, [Password] = @Password, [Age] = @Age, [Gender] = @Gender, [Avatar] = @Avatar WHERE [ID] = @ID">
                        <DeleteParameters>
                            <asp:Parameter Name="ID" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Name" Type="String" />
                            <asp:Parameter Name="Password" Type="String" />
                            <asp:Parameter Name="Age" Type="Int32" />
                            <asp:Parameter Name="Gender" Type="String" />
                            <asp:Parameter Name="Avatar" Type="Object" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Name" Type="String" />
                            <asp:Parameter Name="Password" Type="String" />
                            <asp:Parameter Name="Age" Type="Int32" />
                            <asp:Parameter Name="Gender" Type="String" />
                            <asp:Parameter Name="Avatar" Type="Object" />
                            <asp:Parameter Name="ID" Type="Int32" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </div>

                <!-- 第二个 GridView -->
                <div class="grid-view">
                    <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ConversationId" DataSourceID="SqlDataSource_Converstions" CssClass="auto-style1">
                        <Columns>
                            <asp:BoundField DataField="ConversationId" HeaderText="ConversationId" InsertVisible="False" ReadOnly="True" SortExpression="ConversationId" />
                            <asp:BoundField DataField="ConversationName" HeaderText="ConversationName" SortExpression="ConversationName" />
                            <asp:BoundField DataField="CreatorId" HeaderText="CreatorId" SortExpression="CreatorId" />
                            <asp:BoundField DataField="MaxMembers" HeaderText="MaxMembers" SortExpression="MaxMembers" />
                            <asp:BoundField DataField="MemberCount" HeaderText="MemberCount" SortExpression="MemberCount" />
                            <asp:BoundField DataField="CreatedAt" HeaderText="CreatedAt" SortExpression="CreatedAt" />
                            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        </Columns>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource_Converstions" runat="server" ConnectionString="<%$ ConnectionStrings:OnlineChatConnectionString %>" DeleteCommand="DELETE FROM [Conversations] WHERE [ConversationId] = @ConversationId" InsertCommand="INSERT INTO [Conversations] ([ConversationName], [CreatorId], [MaxMembers], [MemberCount], [CreatedAt], [Avatar], [LastMessageId]) VALUES (@ConversationName, @CreatorId, @MaxMembers, @MemberCount, @CreatedAt, @Avatar, @LastMessageId)" SelectCommand="SELECT * FROM [Conversations]" UpdateCommand="UPDATE [Conversations] SET [ConversationName] = @ConversationName, [CreatorId] = @CreatorId, [MaxMembers] = @MaxMembers, [MemberCount] = @MemberCount, [CreatedAt] = @CreatedAt, [Avatar] = @Avatar, [LastMessageId] = @LastMessageId WHERE [ConversationId] = @ConversationId">
                        <DeleteParameters>
                            <asp:Parameter Name="ConversationId" Type="Int32" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="ConversationName" Type="String" />
                            <asp:Parameter Name="CreatorId" Type="Int32" />
                            <asp:Parameter Name="MaxMembers" Type="String" />
                            <asp:Parameter Name="MemberCount" Type="Int32" />
                            <asp:Parameter DbType="DateTime2" Name="CreatedAt" />
                            <asp:Parameter Name="Avatar" Type="Object" />
                            <asp:Parameter Name="LastMessageId" Type="Int32" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="ConversationName" Type="String" />
                            <asp:Parameter Name="CreatorId" Type="Int32" />
                            <asp:Parameter Name="MaxMembers" Type="String" />
                            <asp:Parameter Name="MemberCount" Type="Int32" />
                            <asp:Parameter DbType="DateTime2" Name="CreatedAt" />
                            <asp:Parameter Name="Avatar" Type="Object" />
                            <asp:Parameter Name="LastMessageId" Type="Int32" />
                            <asp:Parameter Name="ConversationId" Type="Int32" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
