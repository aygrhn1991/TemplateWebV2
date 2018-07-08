using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace TemplateWeb.Component
{
    public class QRTool
    {
        public static string CreateQR(string param)
        {
            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;
            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(param, Encoding.Default);
            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            //输出二维码图片
            return "data:image/png;base64," + Convert.ToBase64String(ms.GetBuffer());
        }
    }
}