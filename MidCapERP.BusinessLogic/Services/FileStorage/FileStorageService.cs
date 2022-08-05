using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MidCapERP.BusinessLogic.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileStorageService(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> StoreFile(IFormFile file, string fileSavingPath)
        {
            string uploadedImagePath = string.Empty;
            string path = _hostingEnvironment.WebRootPath + fileSavingPath;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            using (var stream = new FileStream(path + fileName, FileMode.Create))
            {
                file.CopyToAsync(stream);
                uploadedImagePath = fileSavingPath + fileName;
            }
            return uploadedImagePath;
        }
    }
}