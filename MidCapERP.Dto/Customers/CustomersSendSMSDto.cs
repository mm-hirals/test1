namespace MidCapERP.Dto.Customers
{
    public class CustomersSendSMSDto
    {
        public string CustomerName { get; set; }
        public string CustomerMobileNo { get; set; }
        public DateTime CustomerFromDate { get; set; }
        public DateTime CustomerToDate { get; set; }
        public string Message { get; set; }
        public bool IsCheckedAll { get; set; }
        public List<long> CustomerList { get; set; }
    }
}