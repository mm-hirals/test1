using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Contractors;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapContractors : Profile
    {
        public MapContractors()
        {
            CreateMap<Contractors, ContractorsRequestDto>().ReverseMap();
            CreateMap<Contractors, ContractorsResponseDto>().ReverseMap();
        }
    }
}
