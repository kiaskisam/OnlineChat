using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OnlineChat.User;
using System.Security.Cryptography;
using System.Text;
using Microsoft.SqlServer.Server;


public partial class Login : DbPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }



    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        try
        {
            // 尝试打开数据库连接
            dbConn.Open();
            Console.WriteLine("数据库连接成功！");

            // 创建SqlCommand对象并执行查询
           
            string Name = lName.Text;
            string Psd = SQLCommonTools.ComputeSha256Hash(lPsd.Text);
            string AvatarImageUrl;

            using (SqlCommand cmd = new SqlCommand())
            {
                string SqlCmd = "SELECT * FROM Users WHERE Name=@Name AND Password=@Psd ";
                cmd.CommandText = SqlCmd;
                cmd.Connection = dbConn;

                // 添加参数到SqlCommand对象
                cmd.Parameters.AddWithValue("@Name", Name);
                cmd.Parameters.AddWithValue("@Psd", Psd);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // 检查是否有数据
                    if (reader.HasRows)
                    {
                        User user = new User();
                        // 读取第一行数据
                        if (reader.Read())
                        {
                            // 检查Name列是否为DBNull
                            if (!reader.IsDBNull(reader.GetOrdinal("Name")))
                            {
                                // 读取Name列的值
                                string userName = reader["Name"].ToString();
                                user.Name = userName;
                                user.Password = lPsd.Text;//reader["Password"].ToString();
                                user.Gender=reader["Gender"].ToString();
                                user.Age=(int)reader["Age"];//不使用加密密码
                                user.Id=(int)reader["ID"];

                                
                                error.Text =  "欢迎回来 "+userName+" ，正在跳转中......."; // 显示在error标签中
                               

                                if (!reader.IsDBNull(reader.GetOrdinal("Avatar")))
                                {
                                    // 读取Avatar列的二进制数据
                                    byte[] imageBytes = (byte[])reader["Avatar"];
                                    user.AvatarData=imageBytes;

                                    // 将二进制数据转换为Base64字符串
                                    string base64String = Convert.ToBase64String(imageBytes);

                                    // 设置图片的src属性为Base64编码的字符串
                                    // 假设AvatarImage是一个<img>标签的ID
                                     AvatarImageUrl="data:image/png;base64," + base64String;
                                    AvatarImage.ImageUrl = AvatarImageUrl;
                                    
                                }
                                else
                                {
                                    // 如果没有图片，设置为默认图片
                                    AvatarImageUrl = "Image/DefaultAvatar.jpg";
                                }
                                user.Avatar = AvatarImageUrl;
                            }

                            else
                            {
                                error.Text = "用户名不存在";
                            }
                            Session["User"]=user;
                           
                            // 登录成功，显示提示并延迟跳转
                            string script = 
                                            "setTimeout(function() { window.location = 'ChatList.aspx'; }, 2000);"; // 延迟 2 秒后跳转
                            ClientScript.RegisterStartupScript(this.GetType(), "LoginSuccess", script, true);
                        }
                    }
                    else
                    {
                        // 登录失败逻辑
                        error.Text = "用户名或密码错误";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("数据库连接失败: " + ex.Message);
        }
        finally
        {
            if (dbConn.State == ConnectionState.Open)
            {
                dbConn.Close();
            }
        }

    }


    //密码加密

}