using MidCapERP.Dto.Constants;
using System.ComponentModel;

namespace MidCapERP.Dto.WoodType
{
    public class WoodTypeRequestDto
    {
        public int LookupValueId { get; set; }
        public int LookupId { get; set; } = (int)MasterPagesEnum.WoodType;

        [DisplayName("Wood Type Name")]
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