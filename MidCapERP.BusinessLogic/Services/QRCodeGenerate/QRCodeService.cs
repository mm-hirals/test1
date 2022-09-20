using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
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

        public QRCodeService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<string> GenerateQRCodeImageAsync(string productIdEnc)
        {
            String QrCode = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode("https://localhost:44383/Product/Detail/" + productIdEnc, QRCodeGenerator.ECCLevel.Q);
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