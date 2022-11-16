using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.WrkImportFiles
{
    public class WrkImportFilesRequestDto
    {
        [Required(ErrorMessage ="Please select file")]
        [FileExtensions(Extensions = "csv", ErrorMessage = "Please upload only CSV file")]
        public IFormFile formFile { get; set; }
    }
}