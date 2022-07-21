using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Contractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
