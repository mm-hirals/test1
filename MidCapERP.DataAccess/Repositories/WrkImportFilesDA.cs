using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class WrkImportFilesDA : IWrkImportFilesDA
    {
        private readonly ISqlRepository<WrkImportFiles> _wrkImportFiles;
        private readonly CurrentUser _currentUser;

        public WrkImportFilesDA(ISqlRepository<WrkImportFiles> wrkImportFiles, CurrentUser currentUser)
        {
            _wrkImportFiles = wrkImportFiles;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<WrkImportFiles>> GetAll(CancellationToken cancellationToken)
        {
            return await _wrkImportFiles.GetAsync(cancellationToken, x => x.TenantId == _currentUser.TenantId);
        }

        public async Task<WrkImportFiles> GetById(long Id, CancellationToken cancellationToken)
        {
            return await _wrkImportFiles.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<WrkImportFiles> Create(WrkImportFiles model, CancellationToken cancellationToken)
        {
            return await _wrkImportFiles.InsertAsync(model, cancellationToken);
        }

        public async Task<WrkImportFiles> Update(WrkImportFiles model, CancellationToken cancellationToken)
        {
            return await _wrkImportFiles.UpdateAsync(model, cancellationToken);
        }
    }
}