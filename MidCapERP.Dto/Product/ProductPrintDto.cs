namespace MidCapERP.Dto.Product
{
    public class ProductPrintDto
    {
        public string CategoryName { get; set; }
        public string ProductTitle { get; set; }
        public string ModelNo { get; set; }
        public string PublishStatus { get; set; }
        public bool IsCheckedAll { get; set; }
        public List<long> ProductList { get; set; }
    }
}