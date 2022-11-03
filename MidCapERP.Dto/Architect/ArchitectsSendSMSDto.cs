namespace MidCapERP.Dto.Architect
{
    public class ArchitectsSendSMSDto
    {
        public string CustomerName { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerFromDate { get; set; }
        public string CustomerToDate { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsCheckedAll { get; set; }
        public List<long> CustomerList { get; set; }
    }
}