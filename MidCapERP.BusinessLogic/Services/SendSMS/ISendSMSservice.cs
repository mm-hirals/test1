namespace MidCapERP.BusinessLogic.Services.SendSMS
{
    public interface ISendSMSservice
    {
        public bool SendSMS(string mobilenumber, string message);
    }
}