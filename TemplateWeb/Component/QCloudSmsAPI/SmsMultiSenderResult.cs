using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;


namespace qcloudsms_csharp
{
    public class SmsMultiSenderResult : SmsResultBase
    {
        public class Detail
        {
            public int result;
            public string errmsg = "";
            public string mobile = "";
            public string nationcode = "";
            public string sid = "";
            public int fee;

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }

            public Detail parse(JObject json)
            {
                try
                {
                    result = json.GetValue("result").Value<int>();
                    errmsg = json.GetValue("errmsg").Value<string>();
                }
                catch (ArgumentNullException e)
                {
                    throw new JSONException(String.Format("json: {0}, exception: {1}", json, e.Message));
                }

                if (json["mobile"] != null)
                {
                    mobile = json.GetValue("mobile").Value<string>();
                }
                if (json["nationcode"] != null)
                {
                    nationcode = json.GetValue("nationcode").Value<string>();
                }
                if (json["sid"] != null)
                {
                    sid = json.GetValue("sid").Value<string>();
                }
                if (json["fee"] != null)
                {
                    fee = json.GetValue("fee").Value<int>();
                }

                return this;
            }
        }

        public int result;
        public string errMsg;
        public string ext;
        public List<Detail> details;

        public SmsMultiSenderResult()
        {
            this.errMsg = "";
            this.ext = "";
            this.details = new List<Detail>();
        }

        public override void parseFromHTTPResponse(HTTPResponse response)
        {
            JObject json = parseToJson(response);

            try
            {
                result = json.GetValue("result").Value<int>();
                errMsg = json.GetValue("errmsg").Value<string>();
            }
            catch (ArgumentNullException e)
            {
                throw new JSONException(String.Format("res: {0}, exception: {1}", response.body, e.Message));
            }

            if (json["ext"] != null)
            {
                ext = json.GetValue("ext").Value<string>();
            }
            if (json["detail"] != null)
            {
                foreach (JObject item in json["detail"])
                {
                    details.Add((new Detail()).parse(item));
                }
            }
        }
    }
}