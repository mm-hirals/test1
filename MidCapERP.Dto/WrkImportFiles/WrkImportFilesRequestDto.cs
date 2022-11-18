using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.WrkImportFiles
{
    public class WrkImportFilesRequestDto
    {
        [Required(ErrorMessage = "Please select file")]
        public IFormFile formFile { get; set; }
    }
}