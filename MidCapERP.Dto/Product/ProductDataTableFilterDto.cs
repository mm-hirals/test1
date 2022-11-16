using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Product
{
    public class ProductDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("categoryId")]
        public long CategoryId { get; set; }

        [JsonProperty("productTitle")]
        public string ProductTitle { get; set; }

        [JsonProperty("modelNo")]
        public string ModelNo { get; set; }

        [JsonProperty("publishStatus")]
        public int? publishStatus { get; set; }
    }
}