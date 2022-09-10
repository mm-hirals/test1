using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface ILoginTokenDA
    {
        public Task<IQueryable<LoginToken>> GetAll(CancellationToken cancellationToken);

        public Task<LoginToken> GetById(int Id, CancellationToken cancellationToken);

        public Task<LoginToken> CreateLoginToken(LoginToken model, CancellationToken cancellationToken);

        public Task<LoginToken> UpdateLoginToken(LoginToken model, CancellationToken cancellationToken);
    }
}