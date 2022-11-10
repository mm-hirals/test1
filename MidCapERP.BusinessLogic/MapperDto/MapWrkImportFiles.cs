using AutoMapper;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.WrkImportFiles;

namespace MidCapERP.BusinessLogic.MapperDto
{
    public class MapWrkImportFiles : Profile
    {
        public MapWrkImportFiles()
        {
            CreateMap<WrkImportFiles, WrkImportFilesDto>().ReverseMap();
        }
    }
}