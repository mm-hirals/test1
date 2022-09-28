namespace MidCapERP.Dto.ProductImage
{
    public class ProductImageResponseDto
    {
        public long ProductImageID { get; set; }
        public long ProductId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
    }
}