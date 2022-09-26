namespace MidCapERP.Dto.Customers
{
    public class CustomerForDorpDownByModuleNoResponseDto
    {
        public CustomerForDorpDownByModuleNoResponseDto(long id, string name, string type)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        public Int64 Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
    }
}