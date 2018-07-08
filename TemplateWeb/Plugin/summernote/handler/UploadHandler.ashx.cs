using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TemplateWeb.Plugin.summernote.handler
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            HttpFileCollection files = context.Request.Files;
            if (files.Count <= 0)
            {
                return;
            }
            List<string> urlList = new List<string>();
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];
                string relativePath = "/Upload/editor/";
                string AabsolutePath = context.Server.MapPath(relativePath);
                string filename = String.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}",
                    DateTime.Now.Year,
                    DateTime.Now.Month.ToString("D2"),
                    DateTime.Now.Day.ToString("D2"),
                    DateTime.Now.Hour.ToString("D2"),
                    DateTime.Now.Minute.ToString("D2"),
                    DateTime.Now.Second.ToString("D2"),
                    Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName));
                string imgUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + relativePath + filename;
                urlList.Add(imgUrl);
                if (!Directory.Exists(Path.GetDirectoryName(AabsolutePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(AabsolutePath));
                }
                file.SaveAs(AabsolutePath + filename);
            }
            context.Response.Write(JsonConvert.SerializeObject(urlList));
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}