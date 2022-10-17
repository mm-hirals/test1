using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Product
{
    public class ProductActivityDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("productId")]
        public int productId { get; set; }
    }
}
