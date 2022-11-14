using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.WrkImportCustomers;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapWrkImportCustomers : Profile
    {
        public MapWrkImportCustomers()
        {
            CreateMap<WrkImportCustomers, WrkImportCustomersDto>().ReverseMap();
        }
    }
}