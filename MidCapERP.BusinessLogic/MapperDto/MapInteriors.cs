using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Interior;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapInteriors : Profile
    {
        public MapInteriors()
        {
            CreateMap<Customers, InteriorRequestDto>().ReverseMap();
            CreateMap<Customers, InteriorResponseDto>().ReverseMap();
        }
    }
}