namespace MidCapERP.Dto.Product
{
    public class ProductRequestDto
    {
        public int ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public bool IsVisibleToWholesalers { get; set; }
        public decimal TotalDaysToPrepare { get; set; }
        public int StoreQty { get; set; }
        public string? Comments { get; set; }
        public int? TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}