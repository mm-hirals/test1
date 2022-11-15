using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.ProductQuantities
{
    public class ProductQuantitiesDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("productTitle")]
        public string ProductTitle { get; set; }

        [JsonProperty("quantityDate")]
        public DateTime QuantityDate { get; set; }
    }
}