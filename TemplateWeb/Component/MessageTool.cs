using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateWeb.Models.DB;

namespace TemplateWeb.Component
{
    public class MessageTool
    {
        public static bool SendMessage(int memberId, string title, string content)
        {
            EntityDB entity = new EntityDB();
            member_message message = new member_message()
            {
                content = content,
                member_id = memberId,
                state_read = false,
                sys_datetime = DateTime.Now,
                title = title,
            };
            entity.member_message.Add(message);
            return entity.SaveChanges() > 0;
        }
    }
}