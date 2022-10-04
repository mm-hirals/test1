namespace MidCapERP.Dto.OrderCalculation
{
    public class ProductDimensionsApiResponseDto
    {
        public int SubjectTypeId { get; set; }
        public long SubjectId { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Depth { get; set; }
        public decimal? TotalAmount { get; set; }
        public int Quantity { get; set; }
    }
}