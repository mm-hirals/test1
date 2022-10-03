using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Category
{
    public class CategoryDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("Category Name")]
        public string CategoryName { get; set; }
    }
}