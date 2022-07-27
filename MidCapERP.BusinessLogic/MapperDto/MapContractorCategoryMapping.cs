using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.ContractorCategoryMapping;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapContractorCategoryMapping : Profile
    {
        public MapContractorCategoryMapping()
        {
            CreateMap<ContractorCategoryMapping, ContractorCategoryMappingResponseDto>().ReverseMap();
            CreateMap<ContractorCategoryMapping, ContractorCategoryMappingRequestDto>().ReverseMap();
        }
    }
}