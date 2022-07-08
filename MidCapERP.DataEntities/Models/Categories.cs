using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidCapERP.DataEntities.Models
{
    [Table("Categories")]
    public class Categories : BaseEntity
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}