using MidCapERP.Dto.DataGrid;
using Newtonsoft.Json;

namespace MidCapERP.Dto.TenantBankDetail
{
    public class TenantBankDetailDataTableFilterDto : DataTableFilterDto
    {
        [JsonProperty("TenantId")]
        public int TenantId { get; set; }
    }
}
