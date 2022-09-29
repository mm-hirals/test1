namespace MidCapERP.Dto.Product
{
    public class ProductForDorpDownByModuleNoResponseDto
    {
        public ProductForDorpDownByModuleNoResponseDto(long id, string title, string modelNo, string defaultImage, string type)
        {
            Id = id;
            Title = title;
            ModelNo = modelNo;
            DefaultImage = defaultImage;
            Type = type;
        }

        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string ModelNo { get; set; }
        public string DefaultImage { get; set; }
        public string Type { get; set; }
    }
}