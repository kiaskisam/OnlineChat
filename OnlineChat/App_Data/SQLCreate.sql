CREATE TABLE Users (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255)  NULL,
    Password NVARCHAR(255) NOT NULL,
    Age INT NULL,
	Gender NVARCHAR(59),
    Avatar IMAGE NULL
);

CREATE TABLE Conversations (
    ConversationId INT PRIMARY KEY IDENTITY(1,1),
    ConversationType INT NOT NULL,    -- 使用枚举：1 为一对一，2 为群聊
    CreatorId INT NOT NULL,           -- 会话创建者的用户 ID
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(), -- 会话创建时间
    LastMessageId INT NULL,           -- 最后一条消息的 ID
    FOREIGN KEY (CreatorId) REFERENCES Users(ID),
    FOREIGN KEY (LastMessageId) REFERENCES Messages(MessageId)
);

CREATE TABLE ConversationMembers (
    ConversationId INT NOT NULL,         -- 会话 ID，外键关联到 Conversations 表
    UserId INT NOT NULL,                 -- 用户 ID，外键关联到 Users 表
    Role VARCHAR(50) NOT NULL DEFAULT 'member',  -- 用户角色：'admin' 或 'member' 等
    JoinedAt DATETIME2 NOT NULL DEFAULT GETDATE(),  -- 用户加入会话的时间
    PRIMARY KEY (ConversationId, UserId),  -- 联合主键：会话 ID 和 用户 ID
    FOREIGN KEY (ConversationId) REFERENCES Conversations(ConversationId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);


CREATE TABLE ChatMessages (
    MessageId INT IDENTITY(1,1) PRIMARY KEY,        -- 消息ID
    ConversationId INT NOT NULL,                     -- 会话ID
    SenderId INT NOT NULL,                           -- 发送者ID
        MessageContent NVARCHAR(MAX) NOT NULL,           -- 消息内容（用于文本消息）
    Timestamp DATETIME2 NOT NULL DEFAULT GETDATE(),  -- 消息发送时间
    FOREIGN KEY (ConversationId) REFERENCES Conversations(ConversationId),  -- 外键，指向会话表
    FOREIGN KEY (SenderId) REFERENCES Users(ID), -- 外键，指向用户表
    );

CREATE TABLE AdminUsers (
    AdminUserId INT IDENTITY(1,1) PRIMARY KEY,   -- 自增的管理员用户ID
    Username NVARCHAR(100) NOT NULL,              -- 管理员用户名
    Password NVARCHAR(255) NOT NULL               -- 管理员密码，存储加密后的密码
);
