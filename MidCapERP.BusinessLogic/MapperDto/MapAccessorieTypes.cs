using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.AccessoriesType;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapAccessoriesType : Profile
    {
        public MapAccessoriesType()
        {
            CreateMap<AccessoriesType, AccessoriesTypeResponseDto>().ReverseMap();
            CreateMap<AccessoriesType, AccessoriesTypeRequestDto>().ReverseMap();
        }
    }
}