using OnlineChat.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConversationDetails : System.Web.UI.Page
{
    string connString = @"Data Source=.;Initial Catalog=OnlineChat;Integrated Security=True";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] != null)
        {
            var user = (User)Session["User"];

            // 获取会话ID
            int conversationId = Convert.ToInt32(Request.QueryString["ConversationId"]);

            // 加载会话信息（头像、名称等）
            LoadConversationDetails(conversationId);

            // 加载会话成员
            LoadConversationMembers(conversationId);

            // 检查用户权限，如果有权限显示编辑按钮
            CheckEditPermission(user, conversationId);
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    // 检查用户是否有权限编辑会话
    private void CheckEditPermission(User user, int conversationId)
    {
        bool canEdit = false;

        // 查询 ConversationMembers 表，检查用户是否是该会话的成员或者创建者
        string query = @"
            SELECT Role 
            FROM ConversationMembers 
            WHERE ConversationId = @ConversationId AND UserId = @UserId";
       
        using (SqlConnection dbConn = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(query, dbConn);
            cmd.Parameters.AddWithValue("@ConversationId", conversationId);
            cmd.Parameters.AddWithValue("@UserId", user.Id);

            dbConn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string role = reader["Role"].ToString();  // 获取用户在该会话中的角色

                // 如果角色是 "Creator" 或者是 "Member"（根据需求修改角色）
                if (role == "Creator" || role == "Member")
                {
                    canEdit = true;  // 如果是创建者或成员，显示编辑按钮
                }
            }
            reader.Close();
        }

        // 根据权限控制编辑按钮的显示
        btnEditConversation.Visible = canEdit;
    }
    // 加载会话的基本信息
    private void LoadConversationDetails(int conversationId)
    {
        string query = @"
            SELECT c.ConversationId, c.ConversationName, c.Avatar AS ConversationAvatar, 
                   c.CreatorId, u.Name AS CreatorName 
            FROM Conversations c
            INNER JOIN Users u ON c.CreatorId = u.ID
            WHERE c.ConversationId = @ConversationId";
        
        using (SqlConnection dbConn = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(query, dbConn);
            cmd.Parameters.AddWithValue("@ConversationId", conversationId);

            dbConn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                lblConversationId.Text = reader["ConversationId"].ToString();
                lblCreator.Text = reader["CreatorName"].ToString();
                lblConversationName.Text = reader["ConversationName"].ToString();
                imgConversationAvatar.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])reader["ConversationAvatar"]);
                Session["ConversationAvatar"]=(byte[])reader["ConversationAvatar"];
            }
            reader.Close();
        }
    }

    // 加载会话成员
    private void LoadConversationMembers(int conversationId)
    {
        string query = @"
            SELECT u.Name, u.Avatar
            FROM ConversationMembers cm
            INNER JOIN Users u ON cm.UserId = u.ID
            WHERE cm.ConversationId = @ConversationId";
       
        using (SqlConnection dbConn = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(query, dbConn);
            cmd.Parameters.AddWithValue("@ConversationId", conversationId);

            dbConn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // 将成员信息绑定到 Repeater 控件
            rptMembers.DataSource = dt;
            rptMembers.DataBind();
        }
    }

    // 编辑会话
   
    protected void btnSaveChanges_Click(object sender, EventArgs e)
    {
        string conversationName = txtConversationName.Text.Trim();
        byte[] avatarData = (byte[])Session["ConversationAvatar"];

        // 检查是否上传了头像文件
        if (fuAvatar.HasFile)
        {
            avatarData = SQLCommonTools.SaveAvatar(fuAvatar);  // 获取上传的头像文件（字节数组）
        }

        int conversationId = Convert.ToInt32(Request.QueryString["ConversationId"]);

        // 定义 SQL 更新查询，确保只有上传头像时才更新头像字段
        string query = @"
    UPDATE Conversations 
    SET ConversationName = @ConversationName, 
        Avatar = @Avatar 
    WHERE ConversationId = @ConversationId";

        using (SqlConnection dbConn = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(query, dbConn);

            // 设置参数
            cmd.Parameters.AddWithValue("@ConversationName", conversationName);

            // 如果没有上传头像，传递 DBNull.Value（null）
            // 如果没有头像数据，使用 DBNull.Value，确保不会更新头像字段
            cmd.Parameters.AddWithValue("@Avatar", avatarData ); // 如果没有头像上传，传递 DBNull.Value

            cmd.Parameters.AddWithValue("@ConversationId", conversationId);

            // 执行更新操作
            dbConn.Open();
            cmd.ExecuteNonQuery();
        }

        // 返回会话详情页
        Response.Redirect("ConversationDetails.aspx?ConversationId=" + conversationId);
    
}

    // 返回按钮
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Chatlist.aspx");
    }
}