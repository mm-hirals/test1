using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Category
{
    public class CategoryDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("fixedPrice")]
        public bool FixedPrice { get; set; }
    }
}