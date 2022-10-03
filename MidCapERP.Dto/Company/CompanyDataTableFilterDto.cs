using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Company
{
    public class CompanyDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("Company Name")]
        public string CompanyName { get; set; }

    }
}