using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("ContractorCategoryMapping")]
    public class ContractorCategoryMapping : BaseEntity
    {
        [Key]
        public int ContractorCategoryMappingId { get; set; }
        public int ContractorId { get; set; }
        public int CategoryId { get; set; }
    }
}
