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

        [JsonProperty("columns")]
        public List<DtColumn> Columns { get; set; } = new List<DtColumn>();

        [JsonProperty("search")]
        public DtSearch Search { get; set; } = new DtSearch();

        [JsonProperty("order")]
        public List<DtOrder> Order { get; set; } = new List<DtOrder>();

        /// <summary>
        /// FOR PAGINATION
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize
        { get { return !string.IsNullOrEmpty(Length) ? Convert.ToInt32(this.Length) : 10; } }

        /// <summary>
        /// FOR PAGINATION
        /// </summary>
        [JsonProperty("skip")]
        public int Skip
        { get { return Start; } }

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; } = 0;
    }

    public class DtColumn
    {
        [JsonProperty("data")]
        public string data { get; set; } = String.Empty;

        [JsonProperty("name")]
        public string name { get; set; } = String.Empty;

        [JsonProperty("searchable")]
        public bool searchable { get; set; }

        [JsonProperty("orderable")]
        public bool orderable { get; set; }

        [JsonProperty("search")]
        public DtSearch search { get; set; } = new DtSearch();
    }

    public class DtSearch
    {
        [JsonProperty("value")]
        public string value { get; set; } = String.Empty;

        [JsonProperty("regex")]
        public string regex { get; set; } = String.Empty;
    }

    public class DtOrder
    {
        [JsonProperty("column")]
        public int column { get; set; }

        [JsonProperty("dir")]
        public string dir { get; set; } = String.Empty;
    }
}