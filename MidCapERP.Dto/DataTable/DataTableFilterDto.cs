namespace MidCapERP.Dto.DataGrid
{
    public class DataTableFilterDto
    {
        public string draw { get; set; } = String.Empty;
        public int start { get; set; } = 0;
        public string length { get; set; } = String.Empty;
        public string sortColumn { get; set; } = String.Empty;
        public string sortColumnDirection { get; set; } = String.Empty;
        public string searchValue { get; set; } = String.Empty;
        public int pageSize { get; set; } = 0;
        public int skip { get; set; } = 0;
        public int recordsTotal { get; set; } = 0;
    }
}