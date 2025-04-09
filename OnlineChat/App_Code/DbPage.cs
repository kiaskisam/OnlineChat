using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;  //SqlConnection所在的命名空间
using OnlineChat.User;

/// <summary>
///DbPage 的摘要说明
/// </summary>
public class DbPage: System.Web.UI.Page
{
    /// <summary>
    /// 连接对象成员变量
    /// </summary>
    private SqlConnection _conn = null;
    private User _user = new User();

    public User user { get => _user; set => _user=value; }

    /// <summary>
    /// 获取连接对象
    /// </summary>
    protected SqlConnection dbConn
    {
        get
        {
            if (_conn == null)  //如果尚未创建，则先创建连接对象。
            {
                string connString = @"Data Source=.;Initial Catalog=OnlineChat;Integrated Security=True";
                _conn = new SqlConnection(connString);  //使用连接串，创建连接对象
            }
            return _conn;
        }
    }

}