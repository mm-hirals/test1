using MidCapERP.Dto.WrkImportFiles;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IWrkImportFilesBL
    {
        public Task<IEnumerable<WrkImportFilesDto>> GetAll(CancellationToken cancellationToken);

        public Task<WrkImportFilesDto> GetById(long WrkImportFileID, CancellationToken cancellationToken);

        public Task<WrkImportFilesDto> CreateWrkImportFiles(WrkImportFilesDto model, CancellationToken cancellationToken);
    }
}