using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Fabric;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapFabric : Profile
    {
        public MapFabric()
        {
            CreateMap<Fabric, FabricRequestDto>().ReverseMap();
            CreateMap<Fabric, FabricResponseDto>().ReverseMap();
        }
    }
}