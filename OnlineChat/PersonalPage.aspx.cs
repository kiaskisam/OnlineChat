using OnlineChat.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PersonalPage : DbPage
{
    //User user = new User();
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["User"]!=null) 
        {
             user = Session["User"] as User;
            imgAvatar.ImageUrl = user.Avatar;
            lblUserId.Text=user.Id.ToString();
            lblUserName.Text=user.Name;
            lblAge.Text=user.Age.ToString();
            lblGender.Text=user.Gender;
            //GetUserInfo();
        }
    }

    protected void btnSaveChanges_Click(object sender, EventArgs e)
    {

        // 获取用户ID
        int userId =user.Id;

        // 获取用户输入的修改信息
        string username = string.IsNullOrEmpty(txtUserName.Text) ? user.Name : txtUserName.Text;
        string password = string.IsNullOrEmpty(txtPsd.Text) ? user.Password : txtPsd.Text; // 如果密码为空则使用原来的密码
        int age = string.IsNullOrEmpty(txtAge.Text) ? user.Age : int.Parse(txtAge.Text); // 如果年龄为空则使用原来的年龄
        string gender = ddlGender.SelectedValue; // 性别通常是必填项
        byte[] avatarData = fuAvatar.HasFile?SQLCommonTools.SaveAvatar(fuAvatar):user.AvatarData; // 假设用户上传了头像，保存到数据库
        

        // 调用修改用户信息的数据库方法
        UpdateUserInfo(userId, username, password, age, gender, avatarData);
    }

    // 更新用户信息的方法
    private void UpdateUserInfo(int userId, string username, string password, int age, string gender, byte[] avatarData)
    {
           
            string query = "UPDATE Users SET Name = @UserName, Password = @Password, Age = @Age, Gender = @Gender, Avatar = @Avatar WHERE ID = @UserId";
            using (SqlCommand cmd = new SqlCommand(query, dbConn))
            {
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Password", SQLCommonTools.ComputeSha256Hash(password)); // 假设加密密码
                cmd.Parameters.AddWithValue("@Age", age);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Avatar", avatarData);

                cmd.Parameters.AddWithValue("@UserId", userId); // 使用当前用户ID进行更新

                try
                {
                    Response.Write(cmd.CommandText);
                foreach (SqlParameter param in cmd.Parameters)
                {
                    Response.Write($"Parameter Name: {param.ParameterName}, Value: {param.Value}<br/>");
                }
                dbConn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                    user.Name= username;
                    user.Password= password;
                    user.Age= age;
                    user.Gender= gender;
                    user.AvatarData= avatarData;
                    string base64String = Convert.ToBase64String(avatarData);

                    user.Avatar="data:image/png;base64," + base64String;
                   

                    // 更新成功，提示并跳转到个人主页
                    string script = "alert('修改成功！'); window.location.href='PersonalPage.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "UpdateSuccess", script, true);
                        
                    }
                    else
                    {
                        lberror.Text = "修改失败，请重试。";
                    }
                }
                catch (Exception ex)
                {
                    lberror.Text = "发生错误：" + ex.Message;
                }
            }
        
    }



    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChatList.aspx");
    }

    private void GetUserInfo()
    {

        txtUserName.Text =user.Name;
        txtAge.Text =user.Age.ToString();
        ddlGender.SelectedValue = user.Gender;

    }
}