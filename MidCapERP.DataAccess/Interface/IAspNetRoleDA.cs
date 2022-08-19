using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IAspNetRoleDA
    {
        public Task<IQueryable<ApplicationRole>> GetAll(CancellationToken cancellationToken);
    }
}