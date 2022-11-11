using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.WrkImportFiles;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class WrkImportFilesBL : IWrkImportFilesBL
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        public readonly CurrentUser _currentUser;

        public WrkImportFilesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<WrkImportFilesDto> CreateWrkImportFiles(WrkImportFilesDto model, CancellationToken cancellationToken)
        {
            var wrkFiles = _mapper.Map<WrkImportFiles>(model);
            var data = await _unitOfWorkDA.WrkImportFilesDA.Create(wrkFiles, cancellationToken);
            return _mapper.Map<WrkImportFilesDto>(data);
        }

        public async Task<IEnumerable<WrkImportFilesDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.WrkImportFilesDA.GetAll(cancellationToken);
            return _mapper.Map<List<WrkImportFilesDto>>(data.ToList());
        }

        public async Task<WrkImportFilesDto> GetById(long WrkImportFileID, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.WrkImportFilesDA.GetById(WrkImportFileID, cancellationToken);
            return _mapper.Map<WrkImportFilesDto>(data);
        }
    }
}