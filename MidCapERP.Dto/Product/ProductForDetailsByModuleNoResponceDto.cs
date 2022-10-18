namespace MidCapERP.Dto.Product
{
    public class ProductForDetailsByModuleNoResponceDto
    {
        public ProductForDetailsByModuleNoResponceDto(long productId, int? categoryId, string productTitle, string modelNo, decimal? width, decimal? height, decimal? depth, decimal? diameter, decimal? usedFabric, bool isVisibleToWholesalers, decimal totalDaysToPrepare, string features, string comment, decimal costPrice, string qrImage, int subjectTypeId)
        {
            ProductId = productId;
            CategoryId = categoryId;
            ProductTitle = productTitle;
            ModelNo = modelNo;
            Width = width;
            Height = height;
            Depth = depth;
            Diameter = diameter;
            UsedFabric = usedFabric;
            IsVisibleToWholesalers = isVisibleToWholesalers;
            TotalDaysToPrepare = totalDaysToPrepare;
            Features = features;
            Comments = comment;
            CostPrice = costPrice;
            QRImage = qrImage;
            SubjectTypeId = subjectTypeId;
        }

        public Int64 ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string ProductTitle { get; set; }
        public string ModelNo { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Depth { get; set; }
        public decimal? Diameter { get; set; }
        public decimal? UsedFabric { get; set; }
        public bool IsVisibleToWholesalers { get; set; }
        public decimal TotalDaysToPrepare { get; set; }
        public string Features { get; set; }
        public string Comments { get; set; }
        public decimal CostPrice { get; set; }
        public string QRImage { get; set; }
        public int SubjectTypeId { get; set; }
    }
}