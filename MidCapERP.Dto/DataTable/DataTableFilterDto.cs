using Newtonsoft.Json;

namespace MidCapERP.Dto.DataGrid
{
    public class DataTableFilterDto
    {
        [JsonProperty("draw")]
        public string Draw { get; set; } = String.Empty;

        [JsonProperty("start")]
        public int Start { get; set; } = 0;

        [JsonProperty("length")]
        public string Length { get; set; } = String.Empty;

        [JsonProperty("sortColumn")]
        public string SortColumn { get; set; } = String.Empty;

        [JsonProperty("sortColumnDirection")]
        public string SortColumnDirection { get; set; } = String.Empty;

        [JsonProperty("searchValue")]
        public string SearchValue { get; set; } = String.Empty;

        [JsonProperty("pageSize")]
        public int PageSize
        {
            get
            {
                return Length != null ? Convert.ToInt32(this.Length) : 0;
            }
        }

        [JsonProperty("skip")]
        public int Skip
        {
            get
            {
                return Start;
            }
        }

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; } = 0;
    }
}