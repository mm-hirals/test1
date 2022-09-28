using Microsoft.AspNetCore.Http;

namespace MidCapERP.BusinessLogic.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<string> StoreFile(IFormFile file, string fileSavingPath);
    }
}