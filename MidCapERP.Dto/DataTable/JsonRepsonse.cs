namespace MidCapERP.Dto.DataGrid
{
    public class JsonRepsonse<T> where T : class
    {
        public string draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public List<T> data { get; set; }
    }
}