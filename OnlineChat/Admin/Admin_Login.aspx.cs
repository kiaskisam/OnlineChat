using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Admin_Login : DbPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string checkCode = SQLCommonTools.CreateCode(4);

            //用于验证
            Session["CheckCode"] = checkCode;
            SQLCommonTools.CreateImages(checkCode, Imgcap);
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        string password = txtPassword.Text.Trim();
       

        // 连接数据库获取管理员的密码
        string query = "SELECT Password FROM AdminUsers WHERE Username = @Username";
        if (txtcap.Text != Session["CheckCode"].ToString())
        {
            lblError.Text = "验证码错误，请重试。";
            string checkCode = SQLCommonTools.CreateCode(4);
            Session["CheckCode"] = checkCode;
            SQLCommonTools.CreateImages(checkCode, Imgcap);
            return;
        }
        using (dbConn)
        {
            SqlCommand cmd = new SqlCommand(query, dbConn);
            cmd.Parameters.AddWithValue("@Username", username);

            dbConn.Open();
            var result = cmd.ExecuteScalar();

            if (result != null)
            {
                string storedPasswordHash = result.ToString();

                // 验证输入密码
                if (storedPasswordHash==SQLCommonTools.ComputeSha256Hash(password))
                {
                    // 登录成功，跳转到管理面板
                    Response.Redirect("AdminDashboard.aspx");
                }
                else
                {
                    lblError.Text = "用户名或密码错误。";
                }
            }
            else
            {
                lblError.Text = "用户名不存在。";
            }
        }
    }

 
}
