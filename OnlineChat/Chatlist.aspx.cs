using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineChat.User;
using OnlineChat.ChatMessage;
using System.Drawing;
using System.Net.NetworkInformation;
using System.ServiceModel.Activities;
using System.Text;

public partial class _Default :DbPage
{
    public String ConversationName = "聊天标题";
   // public int ConversationId = 0;每次初始化都会重置以此使用session
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["User"]!=null)
        {
            user = Session["User"] as User;
            //Response.Write(user.Name);
            
            profilePic.ImageUrl=user.Avatar;
            
            LoadUserConversations(user.Id);

            int conversationId = Convert.ToInt32(Request.QueryString["conversationId"]);

            // 获取聊天消息
            List<ChatMessage> messages = GetMessagesByConversationId(conversationId);

            // 渲染聊天消息为 HTML 格式
            StringBuilder sb = new StringBuilder();

            foreach (var message in messages)
            {
                sb.Append("<div class='message'>");
                sb.Append("<img src='" + message.SenderAvatar + "' class='chat-avatar' />");
                sb.Append("<span class='message-sender'>" + message.SenderName + "</span>");
                sb.Append("<div class='message-content'>" + message.Content + "</div>");
                sb.Append("<div class='message-timestamp'>" + message.Timestamp.ToString("HH:mm") + "</div>");
                sb.Append("</div>");
            }

            // 返回渲染后的 HTML
            Response.Write(sb.ToString());
        }
        else
        {
            Response.Write("<script>alert('请先登录'); window.location.href='Login.aspx';</script>");


        }
        
    }
    private void LoadUserConversations(int userId)
    {
        string query = @"
    SELECT 
        cm.ConversationId, 
        c.ConversationName,
        c.Avatar AS ConversationAvatar,  -- 会话头像
        c.CreatorId,
        c.MaxMembers,
        u.Avatar AS CreatorAvatar,       -- 获取创建者头像
        u.Name AS CreatorName            -- 获取创建者名字
    FROM [OnlineChat].[dbo].[ConversationMembers] cm
    INNER JOIN [OnlineChat].[dbo].[Conversations] c
        ON cm.ConversationId = c.ConversationId
    LEFT JOIN [OnlineChat].[dbo].[Users] u
        ON c.CreatorId = u.ID  -- 获取创建者头像和名字
    WHERE cm.UserId = @UserId";
        // 创建 SQL 连接
        using (dbConn)
        {
            SqlCommand cmd = new SqlCommand(query, dbConn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // 遍历查询结果，处理头像显示
            foreach (DataRow row in dt.Rows)
            {
                int conversationId = Convert.ToInt32(row["ConversationId"]);
                int creatorId = Convert.ToInt32(row["CreatorId"]);
                int MaxmemberCount = Convert.ToInt32(row["MaxMembers"]);
                

                //row["ConversationAvatar"] =row["CreatorAvatar"];

                // 判断私聊（会话人数为 2）
                if (MaxmemberCount == 2&&creatorId != userId) // 如果是私聊当前用户不是创建者
                {

                    row["ConversationAvatar"] = row["CreatorAvatar"]; ;  // 设置头像为创建者头像
                    row["ConversationName"]=row["CreatorName"];
                }
                    
            }

            // 将查询结果绑定到 Repeater 控件
            rptChatList.DataSource = dt;
            rptChatList.DataBind();
        }
    }
 


  
    protected void ddlChatAction_SelectedIndexChanged(object sender, EventArgs e)
    {

        string action = ddlChatAction.SelectedValue;
        if (action == "create")
        {
            // 处理创建会话
            Response.Write("<script>alert('创建会话');</script>");
            Response.Redirect("CreateConversation.aspx");
            
        }
        else if (action == "join")
        {
            // 处理加入会话
            Response.Write("<script>alert('加入会话');</script>");
            Response.Redirect("AddConversation.aspx");
        }
    }

  


    //选择会话
    protected void btnSelectConversation_Click(object sender, EventArgs e)
    {
        // 获取拼接的参数字符串
        string commandArgs = ((LinkButton)sender).CommandArgument.ToString();

        // 拆分参数，获取 ID 和 Name
        string[] args = commandArgs.Split(',');

       int ConversationId = Convert.ToInt32(args[0]);  // 获取 ConversationId
        ConversationName = args[1];  // 获取 ConversationName
        Session["ConversationId"]=ConversationId;

        List<ChatMessage> messages = GetMessagesByConversationId(ConversationId);
        rptChatMessages.DataSource = messages;
        rptChatMessages.DataBind();


    }

    //获取点击会话信息
    private List<ChatMessage> GetMessagesByConversationId(int conversationId)
    {
        List<ChatMessage> messages = new List<ChatMessage>();

        // 查询消息并获取发送者的头像和名字
        string query = @"
    SELECT 
        cm.MessageId, 
        cm.SenderId, 
        cm.MessageContent, 
        cm.Timestamp,
        u.Name AS SenderName,  -- 发送者名字
        u.Avatar AS SenderAvatar  -- 发送者头像
    FROM ChatMessages cm
    INNER JOIN Users u ON cm.SenderId = u.ID
    WHERE cm.ConversationId = @ConversationId
    ORDER BY cm.Timestamp";  // 按时间戳排序
        string connString = @"Data Source=.;Initial Catalog=OnlineChat;Integrated Security=True";
        
        using (SqlConnection Conn=new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(query, Conn);
            cmd.Parameters.AddWithValue("@ConversationId", conversationId);

            Conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int messageId = reader.GetInt32(0);           // MessageId
                int senderId = reader.GetInt32(1);            // SenderId
                string content = reader.GetString(2);         // MessageContent
                DateTime timestamp = reader.GetDateTime(3);   // Timestamp
                string senderName = reader.GetString(4);      // SenderName
                byte[] senderAvatar = (byte[])reader["SenderAvatar"];    // SenderAvatar

                // 创建 ChatMessage 对象并添加到列表
                ChatMessage message = new ChatMessage(messageId, conversationId, senderId, senderName, senderAvatar, content, timestamp);
                messages.Add(message);
            }

            reader.Close();
        }

        return messages;
    }


    //消息发送
   

    protected void btnSend_Click(object sender, EventArgs e)
    {
        string messageContent = txtMessage.Text.Trim(); // 获取消息内容

        if (string.IsNullOrEmpty(messageContent))
        {
            // 如果消息内容为空，返回并提示用户
            Response.Write("<script>alert('消息内容不能为空');</script>");
            return;
        }

        int senderId = user.Id; // 获取发送者 ID
        int conversationId = Convert.ToInt32(Session["ConversationId"]); // 会话 ID
       // Response.Write("<script>alert('ConversationId: " + conversationId + "\\nSenderId: " + senderId + "\\nMessageContent: " + messageContent + "');</script>");
        // 将消息内容插入数据库
        bool isMessageSent = SendMessage(conversationId, senderId, messageContent);

        if (isMessageSent)
        {
            // 如果消息发送成功，刷新消息列表
            List<ChatMessage> messages = GetMessagesByConversationId(conversationId);
            rptChatMessages.DataSource = messages;
            rptChatMessages.DataBind();

            // 清空文本框
            txtMessage.Text = "";
        }
        else
        {
            Response.Write("<script>alert('消息发送失败，请稍后再试');</script>");
        }
    }

    private bool SendMessage(int conversationId, int senderId, string messageContent)
    {
        try
        {
            // 插入消息到数据库
            string query = "INSERT INTO ChatMessages (ConversationId, SenderId, MessageContent, Timestamp) VALUES (@ConversationId, @SenderId, @MessageContent, @Timestamp)";
            string connString = @"Data Source=.;Initial Catalog=OnlineChat;Integrated Security=True";
            using (SqlConnection Conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, Conn);
                cmd.Parameters.AddWithValue("@ConversationId", conversationId);
                cmd.Parameters.AddWithValue("@SenderId", senderId);
                cmd.Parameters.AddWithValue("@MessageContent", messageContent);
                cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now); // 当前时间作为消息发送时间

                Conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery(); // 执行插入操作

                if (rowsAffected > 0)
                {
                    return true; // 消息成功插入
                }
                else
                {
                    return false; // 插入失败
                }
            }
        }
        catch (Exception ex)
        {
            // 错误处理
            Response.Write("<script>alert('发生错误: " + ex.Message + "');</script>");
            return false;
        }
    }

}
