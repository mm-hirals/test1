using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Fabric;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapFabric : Profile
    {
        public MapFabric()
        {
            CreateMap<Fabrics, FabricRequestDto>().ReverseMap();
            CreateMap<Fabrics, FabricResponseDto>().ReverseMap();
        }
    }
}