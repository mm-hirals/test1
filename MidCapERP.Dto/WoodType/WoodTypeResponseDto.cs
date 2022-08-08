using System.ComponentModel;

namespace MidCapERP.Dto.WoodType
{
    public class WoodTypeResponseDto
    {
        public int LookupValueId { get; set; }
        public int LookupId { get; set; }

        [DisplayName("Lookup Name")]
        public string LookupName { get; set; }

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