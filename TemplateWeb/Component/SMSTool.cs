using qcloudsms_csharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateWeb.Models.DB;

namespace TemplateWeb.Component
{
    public class SMSTool
    {
        EntityDB entity = new EntityDB();
        public SMSTool()
        {
            var setting = entity.lay_setting;
            this.appId = int.Parse(setting.FirstOrDefault(p => p.key == "qcloudesms_appid").value);
            this.appKey = setting.FirstOrDefault(p => p.key == "qcloudesms_appkey").value;
            this.expireTime = int.Parse(entity.lay_setting.FirstOrDefault(p => p.key == "qcloudesms_expiretime").value);
        }

        private int appId { get; set; }
        private string appKey { get; set; }
        public int expireTime { get; set; }
        public string CreateCode(string phone)
        {
            Random random = new Random();
            string codeStr = random.Next(1000, 10000).ToString();
            account_code account_code = entity.account_code.FirstOrDefault(p => p.phone == phone);
            if (account_code == null)
            {
                account_code new_account_code = new account_code()
                {
                    code = codeStr,
                    phone = phone,
                    sys_datetime = DateTime.Now,
                };
                entity.account_code.Add(new_account_code);
            }
            else
            {
                account_code.code = codeStr;
                account_code.sys_datetime = DateTime.Now;
            }
            entity.SaveChanges();
            return codeStr;
        }
        public bool SendCode(string phone, int templateId, string[] paramters)
        {
            SmsSingleSender ssender = new SmsSingleSender(this.appId, this.appKey);
            var result = ssender.sendWithParam("86", phone, templateId, paramters, null, "", "");  // 签名参数未提供或者为空时，会使用默认签名发送短信
            return result.result == 0;
        }
        public bool CheckCode(string phone, string code)
        {
            account_code account_code = entity.account_code.FirstOrDefault(p => p.phone == phone);
            TimeSpan timeSpan = DateTime.Now.Subtract(account_code.sys_datetime.Value);
            if (timeSpan.TotalMinutes <= this.expireTime)
            {
                return account_code.code == code;
            }
            return false;
        }
    }
}