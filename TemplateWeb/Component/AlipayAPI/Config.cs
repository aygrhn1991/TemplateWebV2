using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aop.Api
{
    public class Config
    {
        public Config()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        // 应用ID,您的APPID
        public static string app_id = "2018050402635049";

        // 支付宝网关
        public static string gatewayUrl = "https://openapi.alipay.com/gateway.do";

        // 商户私钥，您的原始格式RSA私钥
        public static string private_key = "MIIEpAIBAAKCAQEAqAbP7mrEQOO/GArJiSca3Qcmw7vKIxNiMeoGFo8Y2agFUseeJYzpynmwchT4JYIdDDvIbgUDcH/pG5l/0rIjpG0+oG1uAgREWsMi0rGrwU4JVNFVPMDao7+M/GxycOtdrJDfrvrtW/O+Usu25FA5/BtjUIhnL1l/ZH/9KIn28Oc8w+xOAsniSrlLdRIb6MV9R+dtBNj08c4FvwacpCTdGJ8591Uu2Sf0uCkv3BTYPPanYqyKhRKy+KO0vSvAXISKaxim3zcByLrKL9gQL/DciR2WPtWx3beKssuwTePoVfB/1XZX3BNvrYaAziOXOxXmQ8WY8EaT00gdoVXyVthfCwIDAQABAoIBAD0KPH9gSetI0SLqyEwK6Ldi4bI5hjU31XRMj+miEmT/pO9tlQYPKt9jVbYb6Am4OtU24pkVOSqlTQm5ubXRASG6jgW5uz0AT9SpDv17jemrb7hgU5a+eK8D5zPMMcbTujP2bUwj3BaLdZieaNKIUohDatDWvKV23dCkrlMLy6m3wHnZR3n6fHfbApE9UX0K79Lr7vuzWzDapMBI9cQlQtKtDuxQCtRTeHEeFxU5c0eEEf84IZ2VlqRQI5VKfMPscQ9JC3dVe8n3vSLJ9y2HcO1Ym6n35UtOAGRnp/swGdvpWZoAPhbJYX6pFelb3V+jK4BiEIIXkZGgG/qL+LTCWAECgYEA2iMmpfwFbu2szkxAjkBxnwgWSQAPW5M7/uEhZR74weEIBrUZSf3uz1V83JOalMhn8WdE1qLFy6xcx5HW3CArRlbwR5k3QWFXm/VUd3M4WB2Uol5UGWAt8cOnaaS4BZGrqiAx8ZoutcL2pUHbPKoFshHiIxj99CbSksYRSJ3Opc8CgYEAxTEC47C5iuh+yRIcMNCfxdeawS8VvXQDV8gSDhbGLpKaR6GOkeJ3oeUu/WXkJoqsByamqVPxgNNuET1quAmrRuCbsonGFWYlKzDNo9bi20AT3dPoqEivUjLXEUwCXoIrfDWtbkSQ2iyDP9KhDjpZB8UC70I1JLxw7EpSkECpPgUCgYEAuD82dr5u7GouKrW3VS/TiIrxH8nALw4mf7syVNIbNM9Y0ymF6bxlSJSheUHnMEIxoXIjb9NelBDK8Poa7CTzyF7IZwFf7YZF1IIcTSRx8+NQVvCrwh89z1X+ArpRgMqwn2rAJJyhVX8Oizkb75qAYw693f1PCzj7ZWOM17QLBnsCgYBgkbsHhMGo7GlC0TKrGVBVlFv+Za+w40LL13g4HCEcJ8GUeOMYQJ8z0sgrYp3T3i+XJO+Tlz97Ro/ts3UzAdE2WcMGMYCndCmZuEHNDex1UTEpfqC+4TGzAKDgoKe8XuUq5CK2ELAbmJcCR4WGMT7uliX0xCjAf+SRY2iBqpj3fQKBgQCn4WFeQ6QiLOAVh7NHGO6qbFG/6zDrfRPKGsuhJBOjNGn34P8SCccBJ1vS7JDABY5uVhC0az/tiNDA53Lm927tO8nwrfaY1YwGzlS0Jxa3cL6OcizELP9pIzKYJF9pmyBWaR6/+d0IC/VxvS4PLj9lhOpI/HagtkUpOieWWvWKBA==";

        // 支付宝公钥,查看地址：https://openhome.alipay.com/platform/keyManage.htm 对应APPID下的支付宝公钥。
        public static string alipay_public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqAbP7mrEQOO/GArJiSca3Qcmw7vKIxNiMeoGFo8Y2agFUseeJYzpynmwchT4JYIdDDvIbgUDcH/pG5l/0rIjpG0+oG1uAgREWsMi0rGrwU4JVNFVPMDao7+M/GxycOtdrJDfrvrtW/O+Usu25FA5/BtjUIhnL1l/ZH/9KIn28Oc8w+xOAsniSrlLdRIb6MV9R+dtBNj08c4FvwacpCTdGJ8591Uu2Sf0uCkv3BTYPPanYqyKhRKy+KO0vSvAXISKaxim3zcByLrKL9gQL/DciR2WPtWx3beKssuwTePoVfB/1XZX3BNvrYaAziOXOxXmQ8WY8EaT00gdoVXyVthfCwIDAQAB";

        // 签名方式
        public static string sign_type = "RSA2";

        // 编码格式
        public static string charset = "UTF-8";
    }
}