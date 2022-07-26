using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Categories;
using MidCapERP.Dto.SubjectTypes;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapSubjectTypes : Profile
    {
        public MapSubjectTypes()
        {
            CreateMap<SubjectTypes, SubjectTypesResponseDto>().ReverseMap();
            CreateMap<SubjectTypes, SubjectTypesRequestDto>().ReverseMap();
        }
    }
}