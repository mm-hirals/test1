namespace MidCapERP.Dto.Product
{
    public class ProductForDorpDownByModuleNoResponseDto
    {
        public ProductForDorpDownByModuleNoResponseDto(long id, string title, string modelNo, string defaultImage, string productType)
        {
            Id = id;
            Title = title;
            ModelNo = modelNo;
            DefaultImage = defaultImage;
            ProductType = productType;
        }

        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string ModelNo { get; set; }
        public string DefaultImage { get; set; }
        public string ProductType { get; set; }
    }
}