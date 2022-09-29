using System.ComponentModel;

namespace MidCapERP.Dto.Fabric
{
    public class FabricApiResponseDto
    {
        public FabricApiResponseDto(int fabricId, string title, string modelNo, int companyId, int unitId, decimal unitPrice, string? imagePath, int subjectTypeId)
        {
            FabricId = fabricId;
            Title = title;
            ModelNo = modelNo;
            CompanyId = companyId;
            UnitId =  unitId;
            UnitPrice  = unitPrice;
            ImagePath = imagePath;
            SubjectTypeId = subjectTypeId;
        }

        public int FabricId { get; set; }
        public string Title { get; set; }

        [DisplayName("Model No")]
        public string ModelNo { get; set; }

        public int CompanyId { get; set; }

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