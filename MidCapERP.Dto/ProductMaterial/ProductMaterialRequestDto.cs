namespace MidCapERP.Dto.ProductMaterial
{
    public class ProductMaterialRequestDto
    {
        public long ProductMaterialID { get; set; }
        public long ProductId { get; set; }
        public int SubjectTypeId { get; set; }
        public int SubjectId { get; set; }
        public int Qty { get; set; }
        public decimal MaterialPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string? Comments { get; set; }
        public string? UnitType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CreatedUTCDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? UpdatedUTCDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}