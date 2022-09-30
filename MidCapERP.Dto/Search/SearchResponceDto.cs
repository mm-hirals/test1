namespace MidCapERP.Dto.SearchResponse
{
    public class SearchResponse
    {
        public SearchResponse(long id, string title, string number, string defaultImage, string type, int subjectTypeId)
        {
            Id = id;
            Title = title;
            Number = number;
            DefaultImage = defaultImage;
            Type = type;
            SubjectTypeId = subjectTypeId;
        }

        public Int64 Id { get; set; }
        public string Title { get; set; }
        public string Number { get; set; }
        public string DefaultImage { get; set; }
        public string Type { get; set; }
        public int SubjectTypeId { get; set; }
    }
}