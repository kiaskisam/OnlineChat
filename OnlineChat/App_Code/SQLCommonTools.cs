using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Drawing;


/// <summary>
/// 通用工具类
/// </summary>
public static class SQLCommonTools
{
 
    //密码加密
    public static string ComputeSha256Hash(string rawData)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    //保存图片
    public static byte[] SaveAvatar(FileUpload fuAvatar)
    {
        if (fuAvatar.HasFile)
        {
            // 检查文件类型和大小
            string fileExtension = Path.GetExtension(fuAvatar.FileName).ToLower();
            int maxFileSize = 10 * 1024 * 1024; // 限制文件大小为10MB

            if (fuAvatar.PostedFile.ContentLength > maxFileSize)
            {
                Console.Write( "文件大小不能超过2MB。");
                return null;
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                Console.Write( "仅支持上传 .jpg, .jpeg, .png, .gif 格式的文件。");
                return null;
            }

            // 读取文件内容并转换为字节数组
            byte[] avatarData = new byte[fuAvatar.PostedFile.ContentLength];
            fuAvatar.PostedFile.InputStream.Read(avatarData, 0, fuAvatar.PostedFile.ContentLength);
            return avatarData;
        }
        return null;
    }


    //验证码

    public static string CreateCode(int codeLength)
    {
        //去掉不容易识别的字符
        string so = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        string[] strArr = so.Split(',');
        string code = "";
        Random rand = new Random();
        for (int i = 0; i < codeLength; i++)
        {
            code += strArr[rand.Next(0, strArr.Length)];
        }
        return code;
    }

    /*产生验证图片*/
    public static void CreateImages(string code,System.Web.UI.WebControls.Image ImgCap)
    {

        Bitmap image = new Bitmap(70, 26);
        Graphics g = Graphics.FromImage(image);
        WebColorConverter ww = new WebColorConverter();
        g.Clear((Color)ww.ConvertFromString("#eeeeee"));

        Random random = new Random();
        //画图片的背景噪音线
        for (int i = 0; i < 12; i++)
        {
            int x1 = random.Next(image.Width);
            int x2 = random.Next(image.Width);
            int y1 = random.Next(image.Height);
            int y2 = random.Next(image.Height);

            g.DrawLine(new Pen(Color.LightGray), x1, y1, x2, y2);
        }
        Font font = new Font("Arial", 15, FontStyle.Bold | FontStyle.Italic);
        System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(
         new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Gray, 1.2f, true);
        g.DrawString(code, font, brush, 0, 0);

        //画图片的前景噪音点
        for (int i = 0; i < 10; i++)
        {
            int x = random.Next(image.Width);
            int y = random.Next(image.Height);
            image.SetPixel(x, y, Color.White);
        }

        //画图片的边框线
        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        string base64String = Convert.ToBase64String(ms.ToArray());

        // 将Base64字符串赋值给前端控件的ImageUrl属性

        ImgCap.ImageUrl = "data:image/gif;base64," + base64String;
        /* Response.ClearContent();
         Response.ContentType = "image/Gif";
         Response.BinaryWrite(ms.ToArray());*/
        g.Dispose();
        image.Dispose();
    }


    //

}