using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Polish;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapPolish : Profile
    {
        public MapPolish()
        {
            CreateMap<Polish, PolishRequestDto>().ReverseMap();
            CreateMap<Polish, PolishResponseDto>().ReverseMap();
        }
    }
}