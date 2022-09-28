using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.RawMaterial;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapRawMaterial : Profile
    {
        public MapRawMaterial()
        {
            CreateMap<RawMaterial, RawMaterialRequestDto>().ReverseMap();
            CreateMap<RawMaterial, RawMaterialResponseDto>().ReverseMap();
        }
    }
}