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
    ConversationType INT NOT NULL,    -- ʹ��ö�٣�1 Ϊһ��һ��2 ΪȺ��
    CreatorId INT NOT NULL,           -- �Ự�����ߵ��û� ID
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(), -- �Ự����ʱ��
    LastMessageId INT NULL,           -- ���һ����Ϣ�� ID
    FOREIGN KEY (CreatorId) REFERENCES Users(ID),
    FOREIGN KEY (LastMessageId) REFERENCES Messages(MessageId)
);

CREATE TABLE ConversationMembers (
    ConversationId INT NOT NULL,         -- �Ự ID����������� Conversations ��
    UserId INT NOT NULL,                 -- �û� ID����������� Users ��
    Role VARCHAR(50) NOT NULL DEFAULT 'member',  -- �û���ɫ��'admin' �� 'member' ��
    JoinedAt DATETIME2 NOT NULL DEFAULT GETDATE(),  -- �û�����Ự��ʱ��
    PRIMARY KEY (ConversationId, UserId),  -- �����������Ự ID �� �û� ID
    FOREIGN KEY (ConversationId) REFERENCES Conversations(ConversationId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);


CREATE TABLE ChatMessages (
    MessageId INT IDENTITY(1,1) PRIMARY KEY,        -- ��ϢID
    ConversationId INT NOT NULL,                     -- �ỰID
    SenderId INT NOT NULL,                           -- ������ID
        MessageContent NVARCHAR(MAX) NOT NULL,           -- ��Ϣ���ݣ������ı���Ϣ��
    Timestamp DATETIME2 NOT NULL DEFAULT GETDATE(),  -- ��Ϣ����ʱ��
    FOREIGN KEY (ConversationId) REFERENCES Conversations(ConversationId),  -- �����ָ��Ự��
    FOREIGN KEY (SenderId) REFERENCES Users(ID), -- �����ָ���û���
    );

CREATE TABLE AdminUsers (
    AdminUserId INT IDENTITY(1,1) PRIMARY KEY,   -- �����Ĺ���Ա�û�ID
    Username NVARCHAR(100) NOT NULL,              -- ����Ա�û���
    Password NVARCHAR(255) NOT NULL               -- ����Ա���룬�洢���ܺ������
);
