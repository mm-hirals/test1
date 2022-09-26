using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Services.FileStorage;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace MidCapERP.BusinessLogic.Services.QRCodeGenerate
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IConfiguration _configuration;

        public QRCodeService(IFileStorageService fileStorageService, IConfiguration configuration)
        {
            _fileStorageService = fileStorageService;
            _configuration = configuration;
        }

        public async Task<string> GenerateQRCodeImageAsync(string productIdEnc)
        {
            String QrCode = string.Empty;
            string serverPath = _configuration["AppSettings:HostURL"];
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(serverPath + "/Product/Detail/" + productIdEnc, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    //QrCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    IFormFile formFile = new FormFile(ms, 0, ms.Length, "", "QRCode.png");
                    QrCode = await _fileStorageService.StoreFile(formFile, ApplicationFileStorageConstants.FilePaths.QRCode);
                }
            }
            return QrCode;
        }
    }
}