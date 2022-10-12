using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MidCapERP.Dto.Constants;

namespace MidCapERP.Dto.Unit
{
    public class UnitRequestDto
    {
        public int LookupValueId { get; set; }
        public int LookupId { get; set; } = (int)MasterPagesEnum.Unit;
        
        [DisplayName("Unit Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 characters, Maximum 50 characters")]
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