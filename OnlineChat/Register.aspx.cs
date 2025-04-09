using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

public partial class Register : DbPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        // error.Text=Session["CheckCode"].ToString();

        if (!IsPostBack)
        {
            string checkCode = SQLCommonTools.CreateCode(4);

            //用于验证
            Session["CheckCode"] = checkCode;
            SQLCommonTools.CreateImages(checkCode, ImgCap);
        }
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        // 验证验证码
        if (tCaptcha.Text != Session["CheckCode"].ToString())
        {
            error.Text = "验证码错误，请重试。";
            string checkCode = SQLCommonTools.CreateCode(4);
            Session["CheckCode"] = checkCode;
            SQLCommonTools.CreateImages(checkCode,ImgCap);
            return;
        }
        error.Text = "验证码正确";

        // 获取用户输入
        string username = txtName.Text;
        string password = txtPassword.Text; // 应该加密存储
        int age = string.IsNullOrEmpty(txtAge.Text) ? 0 : int.Parse(txtAge.Text); // 检查能否为空，为空则赋值为0
        string gender = ddlGender.SelectedValue;
        byte[] avatarData = SQLCommonTools.SaveAvatar(fuAvatar); // 假设这个方法返回头像的字节数组

        // 注册用户逻辑
        if (Page.IsValid)
        {
            try
            {
                dbConn.Open();
                string query = "INSERT INTO Users (Name, Password, Age, Gender, Avatar) VALUES (@UserName, @Password, @Age, @Gender, @Avatar)";
                using (SqlCommand cmd = new SqlCommand(query, dbConn))
                {
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Password", SQLCommonTools.ComputeSha256Hash(password)); // 加密密码
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@Gender", gender);

                    // 检查头像是否为空
                    if (avatarData != null && avatarData.Length > 0)
                    {
                        cmd.Parameters.AddWithValue("@Avatar", avatarData);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Avatar", DBNull.Value); // 如果没有头像，则插入 DBNull
                    }

                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        // 注册成功，跳转到登录页面
                   
                        string script = "alert('注册成功！正在跳转...');" +
                                        "setTimeout(function() { window.location = 'Login.aspx.aspx'; }, 3000);"; // 延迟 3 秒后跳转
                        ClientScript.RegisterStartupScript(this.GetType(), "RegisterSuccess", script, true);
                       // Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        error.Text = "注册失败，请重试。";
                    }
                }
            }
            catch (Exception ex)
            {
                error.Text = "发生错误：" + ex.Message;
            }
            finally
            {
                // 确保数据库连接被关闭
                if (dbConn.State == ConnectionState.Open)
                {
                    dbConn.Close();
                }
            }
        }
    }

   
}