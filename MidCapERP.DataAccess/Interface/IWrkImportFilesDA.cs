using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IWrkImportFilesDA
    {
        public Task<IQueryable<WrkImportFiles>> GetAll(CancellationToken cancellationToken);

        public Task<WrkImportFiles> GetById(Int64 Id, CancellationToken cancellationToken);

        public Task<WrkImportFiles> Create(WrkImportFiles model, CancellationToken cancellationToken);

        public Task<WrkImportFiles> Update(WrkImportFiles model, CancellationToken cancellationToken);
    }
}