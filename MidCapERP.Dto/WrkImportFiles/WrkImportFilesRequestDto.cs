using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.WrkImportFiles
{
    public class WrkImportFilesRequestDto
    {
        [Required]
        public IFormFile formFile { get; set; }
    }
}