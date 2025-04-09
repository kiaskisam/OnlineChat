using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;

/// <summary>
/// 网站用户信息
/// </summary>
///

namespace OnlineChat.User
{
    public class User
    {
        private string _Name, _Password, _Gender, _Avatar;
        private int _Id, _Age;
        private byte[] _AvatarData;
        

        public User(string name = null, string password = null, string gender = null, int age = 0, int id = 0, string avatar = null, byte[] avtardata=null)
        {
            _Name=name;
            _Password=password;
            _Gender=gender;
            _Age=age;
            _Id=id;
            _Avatar=avatar;//存储URL
            _AvatarData=avtardata;


        }

        public string Name { get => _Name; set => _Name=value; }
        public string Password { get => _Password; set => _Password=value; }
        public string Gender { get => _Gender; set => _Gender=value; }
        public int Id { get => _Id; set => _Id=value; }
        public int Age { get => _Age; set => _Age=value; }
        public string Avatar { get => _Avatar; set => _Avatar=value; }

        public byte[] AvatarData { get => _AvatarData; set => _AvatarData=value; }
    }

}