using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ChatMessage 的摘要说明
/// </summary>
/// 
namespace OnlineChat.ChatMessage
{
    public class ChatMessage
    {
        public int MessageId { get; set; }            // 消息 ID
        public int ConversationId { get; set; }       // 会话 ID
        public int SenderId { get; set; }             // 发送者 ID
        public string SenderName { get; set; }        // 发送者名字
        public byte[] SenderAvatar { get; set; }      // 发送者头像（路径或 Base64）
        public string Content { get; set; }           // 消息内容
        public DateTime Timestamp { get; set; }       // 消息发送时间

        // 构造函数
        public ChatMessage(int messageId, int conversationId, int senderId, string senderName, byte[] senderAvatar, string content, DateTime timestamp)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            SenderId = senderId;
            SenderName = senderName;
            SenderAvatar = senderAvatar;
            Content = content;
            Timestamp = timestamp;
        }
    }
}