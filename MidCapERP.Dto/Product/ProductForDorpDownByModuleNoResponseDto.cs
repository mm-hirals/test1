namespace MidCapERP.Dto.Product
{
    public class ProductForDorpDownByModuleNoResponseDto
    {
        public ProductForDorpDownByModuleNoResponseDto(long productId, string productTitle, string modelNo)
        {
            ProductId = productId;
            ProductTitle = productTitle;
            ModelNo = modelNo;
        }

        public Int64 ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ModelNo { get; set; }
    }
}