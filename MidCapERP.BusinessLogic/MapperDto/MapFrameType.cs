using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.FrameType;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapFrameType : Profile
    {
        public MapFrameType()
        {
            CreateMap<LookupValues, FrameTypeRequestDto>().ReverseMap();
            CreateMap<LookupValues, FrameTypeResponseDto>().ReverseMap();
        }
    }
}