using Microsoft.Extensions.Configuration;
using System.Net;
using WhatsAppApi;

namespace MidCapERP.BusinessLogic.Services.SendSMS
{
    public class SendSMSservice : ISendSMSservice
    {
        public string SenderId;
        public string SMSType;
        public string APIkey;
        private readonly IConfiguration _configuration;

        public SendSMSservice(IConfiguration configuration)
        {
            SenderId = "BOOKCL";
            SMSType = "4";
            APIkey = "OCZs4VSRiV8lNVIYvjdzl4RWbo";
            _configuration = configuration;
        }

        public bool SendSMS(string mobilenumber, string message)
        {
            HttpWebRequest objWebRequest = null;
            HttpWebResponse objWebResponse = null;
            StreamReader objStreamReader = null;
            string Resp = "";
            try
            {
                string API = "" + APIkey + "&SenderID=" + SenderId + "&SMSType=" + SMSType + "&Mobile=" + mobilenumber + "&MsgText=" + message;
                //string API = "" + APIkey + "&SenderID=" + SenderId + "&SMSType=" + SMSType + "&Mobile=" + mobilenumber + "&MsgText=" + message;
                objWebRequest = (HttpWebRequest)WebRequest.Create(API);
                objWebRequest.Method = "GET";
                objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();
                objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                Resp = objStreamReader.ReadToEnd();
                objStreamReader.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool WhatsAppSendGreet(string mobilenumber, string message)
        {
            string from = _configuration["AppSettings:WhatsAppSetting:fromNumber"];
            string password = _configuration["AppSettings:WhatsAppSetting:Password"];
            string nickName = _configuration["AppSettings:WhatsAppSetting:NickName"];
            string to = mobilenumber;//Sender Mobile
            string msg = message;

            WhatsApp wa = new WhatsApp(from, password, nickName, true, true);

            wa.OnConnectSuccess += () =>
            {
                //MessageBox.Show("Connected to whatsapp...");
                wa.OnLoginSuccess += (phoneNumber, data) =>
                {
                    wa.SendMessage(to, msg);
                    //MessageBox.Show("Message Sent...");
                };

                wa.OnLoginFailed += (data) =>
                {
                    //MessageBox.Show("Login Failed : {0}", data);
                };

                wa.Login();
            };

            wa.OnConnectFailed += (ex) =>
            {
                //MessageBox.Show("Connection Failed...");
            };

            wa.Connect();
            return true;
        }
    }
}