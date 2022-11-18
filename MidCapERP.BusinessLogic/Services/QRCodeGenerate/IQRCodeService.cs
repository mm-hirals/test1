namespace MidCapERP.BusinessLogic.Services.QRCodeGenerate
{
    public interface IQRCodeService
    {
        public Task<string> GenerateQRCodeImageAsync(string productIdEnc, string tenantLogo);
    }
}