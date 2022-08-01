using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.AccessoriesTypes;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapAccessoriesTypes : Profile
    {
        public MapAccessoriesTypes()
        {
            CreateMap<AccessoriesTypes, AccessoriesTypesResponseDto>().ReverseMap();
            CreateMap<AccessoriesTypes, AccessoriesTypesRequestDto>().ReverseMap();
        }
    }
}