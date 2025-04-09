using OnlineChat.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChatAdd : DbPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User"]!=null)
        {
            user=Session["User"] as User;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // 获取用户输入的搜索关键字

        string search_Text = searchText.Text.Trim();

        // 如果没有输入任何关键字，则返回
        if (string.IsNullOrEmpty(search_Text))
        {
            // 可以返回一个错误提示
            return;
        }

        // 获取搜索结果
        DataTable searchResults = SearchConversations(search_Text);

        // 将查询结果绑定到 Repeater 控件
        rptSearchResults.DataSource = searchResults;
        rptSearchResults.DataBind();
    }
    private DataTable SearchConversations(string searchText)
    {
        // 定义查询语句，搜索会话名称或 ID
        string query = @"
            SELECT 
            c.ConversationId,
            c.ConversationName AS Title,
            c.Avatar
        FROM Conversations c
        WHERE c.ConversationName LIKE @SearchText OR c.ConversationId LIKE @SearchText";

        // 创建 SQL 连接并执行查询
        using (dbConn)
        {
            SqlCommand cmd = new SqlCommand(query, dbConn);
            cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt;
        }
    }
    protected void AddChat_Click(object sender, EventArgs e)
    {
        // 获取用户选择的会话 ID
        Button btnAddChat = (Button)sender;
        int conversationId = Convert.ToInt32(btnAddChat.CommandArgument);

        //获取用户ID
        int userId = user.Id; 

        // 执行将用户加入会话的操作
        bool isAdded = AddUserToConversation(userId, conversationId);

        if (isAdded)
        {
            // 成功加入会话，显示成功消息
            Response.Write("<script>alert('加入会话成功！');</script>");
        }
        else
        {
            // 失败处理
            Response.Write("<script>alert('加入会话失败，请稍后再试。');</script>");
        }
    }

    // 将用户加入会话的方法
    private bool AddUserToConversation(int userId, int conversationId)
    {
        string query = @"
            INSERT INTO ConversationMembers (ConversationId, UserId, Role, JoinedAt)
            VALUES (@ConversationId, @UserId, 'Member', GETDATE())";

        using (dbConn)
        {
            SqlCommand cmd = new SqlCommand(query, dbConn);
            cmd.Parameters.AddWithValue("@ConversationId", conversationId);
            cmd.Parameters.AddWithValue("@UserId", userId);

            try
            {
                dbConn.Open();
                cmd.ExecuteNonQuery();
                string script = "alert('会话加入成功！'); window.location.href='ChatList.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "CreateConversationSuccess", script, true);
                return true; // 成功加入

            }
            catch (Exception ex)
            {
                // 失败时可以记录日志或显示详细错误
                return false; // 加入失败
            }
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChatList.aspx");
    }
}