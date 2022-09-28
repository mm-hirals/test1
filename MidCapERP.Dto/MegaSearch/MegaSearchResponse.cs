namespace MidCapERP.Dto.MegaSearch
{
    public class MegaSearchResponse
    {
        public MegaSearchResponse(long id, string title, string number, string defaultImage, string type)
        {
            Id = id;
            Title = title;
            Number = number;
            DefaultImage = defaultImage;
            Type = type;
        }

        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string Number { get; set; }
        public string DefaultImage { get; set; }
        public string Type { get; set; }
    }
}