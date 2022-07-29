using Newtonsoft.Json;

namespace MidCapERP.Dto.DataGrid
{
    public class JsonRepsonse<T> where T : class
    {
        public JsonRepsonse(string draw, int recordsFiltered, int recordsTotal, List<T> data)
        {
            Draw = draw;
            RecordsFiltered = recordsFiltered;
            RecordsTotal = recordsTotal;
            Data = data;
        }

        [JsonProperty("draw")]
        public string Draw { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }
}