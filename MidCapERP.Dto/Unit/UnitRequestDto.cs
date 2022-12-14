using System.ComponentModel;
using MidCapERP.Dto.Constants;

namespace MidCapERP.Dto.Unit
{
    public class UnitRequestDto
    {
        public int LookupValueId { get; set; }
        public int LookupId { get; set; } = (int)MasterPagesEnum.Unit;
        
        [DisplayName("Unit Name")]
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