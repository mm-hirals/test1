using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Frames;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapFrame : Profile
    {
        public MapFrame()
        {
            CreateMap<Frames, FrameRequestDto>().ReverseMap();
            CreateMap<Frames, FrameResponseDto>().ReverseMap();
        }
    }
}