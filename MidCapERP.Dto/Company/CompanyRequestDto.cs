using MidCapERP.Dto.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Company
{
    public class CompanyRequestDto
    {
        public int LookupValueId { get; set; }
        public int LookupId { get; set; } = (int)MasterPagesEnum.Company;

        [DisplayName("Company Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Maximum 50 characters")]
        public string LookupValueName { get; set; }

        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}
