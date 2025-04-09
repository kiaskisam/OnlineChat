using OnlineChat.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateConversation : DbPage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"]!=null)
        {
            user=Session["User"] as User;
        }
    }



    protected void btnCreateConversation_Click(object sender, EventArgs e)
    {
        // 获取用户输入的信息
        // 获取用户输入的信息
        string conversationName = string.IsNullOrEmpty(txtConversationName.Text.Trim()) ? "默认会话名称" : txtConversationName.Text.Trim();
        bool isPrivateChat = chkPrivateChat.Checked;
        string privateId = isPrivateChat && string.IsNullOrEmpty(txtPrivateId.Text.Trim()) ? "无" : txtPrivateId.Text.Trim();

        // 获取最大人数，如果为空则默认为 2，否则使用用户输入的值
        int maxParticipants = string.IsNullOrEmpty(txtMaxParticipants.Text.Trim())? 2: (int.Parse(txtMaxParticipants.Text.Trim()));  // 如果不为空，则使用用户输入，若无效则默认为 2


        // 获取当前用户的 ID（假设从 Session 获取）
        int creatorId =user.Id;

        // 获取头像
        byte[] avatarData = SQLCommonTools.SaveAvatar(fileUploadAvatar); // 获取上传的头像（字节数组）

        // 使用事务插入数据
        InsertConversationWithTransaction(conversationName, creatorId, maxParticipants, isPrivateChat, privateId, avatarData);
    }

    // 使用事务插入会话和会话成员
    private void InsertConversationWithTransaction(string conversationName, int creatorId, int maxParticipants, bool isPrivateChat, string privateId, byte[] avatarData)
    {
        using (dbConn )
        {
            // 开始一个新的事务
            SqlTransaction transaction = null;

            try
            {
                // 打开数据库连接
                dbConn.Open();

                // 开始事务
                transaction = dbConn.BeginTransaction();

                if (!IsUserExists(creatorId, dbConn, transaction))
                {
                    throw new Exception("用户ID不存在，无法创建会话。");
                }

                // 获取私聊对象头像（如果是私聊）
                byte[] conversationAvatar = avatarData;

                if (isPrivateChat && !string.IsNullOrEmpty(privateId))
                {
                    // 获取私聊对象头像
                    int privateUserId = int.Parse(privateId); // 假设用户输入的 ID 是有效的
                    conversationAvatar = GetUserAvatar(privateUserId, dbConn, transaction);
                }

                // 插入会话
                int conversationId = InsertConversation(conversationName, creatorId, maxParticipants, isPrivateChat, privateId, conversationAvatar, dbConn, transaction);


                
                // 插入会话成员（包括创建者）
                InsertConversationMember(conversationId, creatorId, "Creator", dbConn, transaction);

                // 如果是私聊，还需要插入私聊对象
                if (isPrivateChat)
                {
                    int privateUserId = int.Parse(privateId); // 假设用户输入的 ID 是有效的
                    InsertConversationMember(conversationId, privateUserId, "Member", dbConn, transaction);
                }

                // 提交事务
                transaction.Commit();

                // 显示成功提示，并跳转到会话列表页面
                string script = "alert('会话创建成功！'); window.location.href='ChatList.aspx';";
                ClientScript.RegisterStartupScript(this.GetType(), "CreateConversationSuccess", script, true);
            }
            catch (Exception ex)
            {
                // 发生错误时回滚事务
                if (transaction != null)
                {
                    transaction.Rollback(); // 回滚事务
                }
                error.Text = "创建会话失败：" + ex.Message;
                Console.WriteLine("数据库操作失败：" + ex.Message);
            }
        } // `using` 块会自动关闭连接
    }

    // 插入会话数据到 Conversations 表
    private int InsertConversation(string conversationName, int creatorId, int maxParticipants, bool isPrivateChat, string privateId, byte[] avatarData, SqlConnection dbConn, SqlTransaction transaction)
    {
        string query = "INSERT INTO Conversations (ConversationName, CreatorId, MaxMembers, MemberCount, CreatedAt, Avatar, LastMessageId) " +
                       "VALUES (@ConversationName, @CreatorId, @MaxMembers, @MemberCount, GETDATE(), @Avatar, @LastMessageId); " +
                       "SELECT SCOPE_IDENTITY();"; // 获取插入后的 ConversationId

        using (SqlCommand cmd = new SqlCommand(query, dbConn, transaction))
        {
            cmd.Parameters.AddWithValue("@ConversationName", conversationName);
            cmd.Parameters.AddWithValue("@CreatorId", creatorId);
            cmd.Parameters.AddWithValue("@MaxMembers", maxParticipants);
            cmd.Parameters.AddWithValue("@MemberCount", 1); // 初始时，成员数为 1（创建者是第一个成员）

            if (avatarData != null && avatarData.Length > 0)
            {
                cmd.Parameters.AddWithValue("@Avatar", avatarData); // 如果头像存在，插入头像数据
            }
            else
            {
                cmd.Parameters.AddWithValue("@Avatar", DBNull.Value); // 如果没有头像，插入 DBNull
            }
            cmd.Parameters.AddWithValue("@LastMessageId", DBNull.Value); // 初始没有消息，LastMessageId 设置为 NULL

            int conversationId = Convert.ToInt32(cmd.ExecuteScalar());
            return conversationId;
        }
    }

    // 插入会话成员数据到 ConversationMembers 表
    private void InsertConversationMember(int conversationId, int userId, string role, SqlConnection dbConn, SqlTransaction transaction)
    {
        string query = "INSERT INTO ConversationMembers (ConversationId, UserId, Role, JoinedAt) " +
                       "VALUES (@ConversationId, @UserId, @Role, GETDATE())";

        using (SqlCommand cmd = new SqlCommand(query, dbConn, transaction))
        {
            cmd.Parameters.AddWithValue("@ConversationId", conversationId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Role", role);

            cmd.ExecuteNonQuery();
        }
    }

    private byte[] GetUserAvatar(int userId, SqlConnection dbConn, SqlTransaction transaction)
    {
        string query = "SELECT Avatar FROM Users WHERE ID = @UserId";

        using (SqlCommand cmd = new SqlCommand(query, dbConn, transaction))
        {
            cmd.Parameters.AddWithValue("@UserId", userId);

            // 执行查询并获取头像数据
            object result = cmd.ExecuteScalar();

            // 如果用户没有头像，返回 null
            return result != DBNull.Value ? (byte[])result : null;
        }
    }

    // 检查指定的 UserId 是否在 Users 表中存在
    private bool IsUserExists(int userId, SqlConnection dbConn, SqlTransaction transaction)
    {
        string query = "SELECT COUNT(*) FROM Users WHERE ID = @UserId";

        using (SqlCommand cmd = new SqlCommand(query, dbConn, transaction))
        {
            cmd.Parameters.AddWithValue("@UserId", userId);

            // 执行查询并返回用户的数量
            int userCount = (int)cmd.ExecuteScalar();

            // 如果返回的用户数量大于 0，表示用户存在
            return userCount > 0;
        }
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChatList.aspx");
    }

    protected void chkPrivateChat_CheckedChanged(object sender, EventArgs e)
    {

    }
}