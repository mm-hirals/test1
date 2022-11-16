using MidCapERP.Dto.ActivityLogs;
using System.ComponentModel;

namespace MidCapERP.Dto.ProductQuantities
{
    public class ProductQuantitiesRequestDto
    {
        public long ProductQuantityId { get; set; }
        public long ProductId { get; set; }

        [DisplayName("Product Name")]
        public string ProductTitle { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        public DateTime QuantityDate { get; set; }

        [DisplayName("As of Quantity Date")]
        public string QuantityDateFormat => QuantityDate.ToShortDateString();

        [DisplayName("Current Quantity")]
        public int Quantity { get; set; }

        [DisplayName("Updated Quantity")]
        public int UpdatedQuantity { get; set; }

        public long LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime LastModifiedUTCDate { get; set; }

        public List<ActivityLogsResponseDto> activityLogs { get; set; }
    }
}