using System.Net;

namespace MidCapERP.BusinessLogic.Services.SendSMS
{
    public class SendSMSservice : ISendSMSservice
    {
        public string SenderId;
        public string SMSType;
        public string APIkey;

        public SendSMSservice()
        {
            SenderId = "BOOKCL";
            SMSType = "4";
            APIkey = "OCZs4VSRiV8lNVIYvjdzl4RWbo";
        }

        public bool SendSMS(string mobilenumber, string message)
        {
            //HttpWebRequest objWebRequest = null;
            //HttpWebResponse objWebResponse = null;
            //StreamReader objStreamReader = null;
            //string Resp = "";
            //try
            //{
            //    string API = "" + APIkey + "&SenderID=" + SenderId + "&SMSType=" + SMSType + "&Mobile=" + mobilenumber + "&MsgText=" + message;
            //    //string API = "" + APIkey + "&SenderID=" + SenderId + "&SMSType=" + SMSType + "&Mobile=" + mobilenumber + "&MsgText=" + message;
            //    objWebRequest = (HttpWebRequest)WebRequest.Create(API);
            //    objWebRequest.Method = "GET";
            //    objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();
            //    objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
            //    Resp = objStreamReader.ReadToEnd();
            //    objStreamReader.Close();
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}

            string mobile = "Way2SMSMobileNo";
            string password = "Way2SMSPassword";
            string text = "test";
            string number = "8460131814";
            string key = "API_Key";

            string url = "https://smsapi.engineeringtgr.com/send/?Mobile=" + mobile + "&Password=" + password + "&Message=" + text + "&To=" + number + "&Key=" + key;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            ServicePointManager.DefaultConnectionLimit = 9999;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.ContentLength = 0;

            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                responseReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}