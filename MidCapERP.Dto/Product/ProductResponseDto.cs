namespace MidCapERP.Dto.Product
{
    public class ProductResponseDto
    {
        public Int64 ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string ProductTitle { get; set; }
        public string ModelNo { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public decimal? UsedFabric { get; set; }
        public decimal? UsedPolish { get; set; }
        public bool IsVisibleToWholesalers { get; set; }
        public decimal TotalDaysToPrepare { get; set; }
        public string Features { get; set; }
        public string Comments { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailerPrice { get; set; }
        public decimal WholesalerPrice { get; set; }
        public string? CoverImage { get; set; }
        public string? QRImage { get; set; }
        public int TenantId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
    }
}