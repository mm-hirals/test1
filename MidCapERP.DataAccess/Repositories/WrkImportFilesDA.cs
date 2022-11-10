using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Repositories
{
    public class WrkImportFilesDA : IWrkImportFilesDA
    {
        private readonly ISqlRepository<WrkImportFiles> _wrkImportFiles;

        public WrkImportFilesDA(ISqlRepository<WrkImportFiles> wrkImportFiles)
        {
            _wrkImportFiles = wrkImportFiles;
        }

        public async Task<WrkImportFiles> Create(WrkImportFiles model, CancellationToken cancellationToken)
        {
            return await _wrkImportFiles.InsertAsync(model, cancellationToken);
        }

        public async Task<WrkImportFiles> Update(WrkImportFiles model, CancellationToken cancellationToken)
        {
            return await _wrkImportFiles.UpdateAsync(model, cancellationToken);
        }

        public async Task<IQueryable<WrkImportFiles>> GetAll(CancellationToken cancellationToken)
        {
            return await _wrkImportFiles.GetAsync(cancellationToken);
        }

        public async Task<WrkImportFiles> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _wrkImportFiles.GetByIdAsync(Id, cancellationToken);
        }
    }
}