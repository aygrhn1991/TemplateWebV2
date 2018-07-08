using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TemplateWeb.Component
{
    public class DESTool
    {
        private const string KEY = "12345678";//必须8位
        //加密
        public static string Encrypt(string str)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(KEY);
            byte[] ivByte = Encoding.UTF8.GetBytes(KEY);
            byte[] strByte = Encoding.UTF8.GetBytes(str);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(keyByte, ivByte), CryptoStreamMode.Write))
                {
                    cs.Write(strByte, 0, strByte.Length);
                    cs.FlushFinalBlock();
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        //解密
        public static string Decrypt(string str)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(KEY);
            byte[] ivByte = Encoding.UTF8.GetBytes(KEY);
            byte[] strByte = Convert.FromBase64String(str);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(keyByte, ivByte), CryptoStreamMode.Write))
                {
                    cs.Write(strByte, 0, strByte.Length);
                    cs.FlushFinalBlock();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}