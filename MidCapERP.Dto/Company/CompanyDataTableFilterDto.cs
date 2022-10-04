using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.Company
{
    public class CompanyDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

    }
}