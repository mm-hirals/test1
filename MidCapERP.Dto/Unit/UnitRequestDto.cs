using Microsoft.AspNetCore.Mvc;
using MidCapERP.Dto.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MidCapERP.Dto.Unit
{
    public class UnitRequestDto
    {
        public int LookupValueId { get; set; }
        public int LookupId { get; set; } = (int)MasterPagesEnum.Unit;

        [DisplayName("Unit Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Minimum 2 characters, Maximum 50 characters")]
        [Remote("DuplicateUnitName", "Unit", AdditionalFields = nameof(LookupValueId), ErrorMessage = "Unit Name already exist. Please enter a different Unit Name.")]
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