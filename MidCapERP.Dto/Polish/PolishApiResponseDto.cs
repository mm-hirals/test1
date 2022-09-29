using System.ComponentModel;

namespace MidCapERP.Dto.Polish
{
    public class PolishApiResponseDto
    {
        public PolishApiResponseDto(int polishId, string title, string modelNo, int companyId, int unitId, decimal unitPrice, string? imagePath, int subjectTypeId)
        {
            PolishId = polishId;
            Title = title;
            ModelNo = modelNo;
            CompanyId = companyId;
            UnitId = unitId;
            UnitPrice = unitPrice;
            ImagePath = imagePath;
            SubjectTypeId = subjectTypeId;
        }

        public int PolishId { get; set; }
        public string Title { get; set; }

        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        public int CompanyId { get; set; }

        [DisplayName("Company Name")]
        public string CompanyName { get; set; }

        public int UnitId { get; set; }

        [DisplayName("Unit Name")]
        public string UnitName { get; set; }

        [DisplayName("Unit Price")]
        public decimal UnitPrice { get; set; }

        [DisplayName("Photo Upload")]
        public string? ImagePath { get; set; }

        public int SubjectTypeId { get; set; }
    }
}
