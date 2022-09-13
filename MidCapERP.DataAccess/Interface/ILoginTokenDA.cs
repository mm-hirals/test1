using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IOTPLoginDA
    {
        public Task<IQueryable<OTPLogin>> GetAll(CancellationToken cancellationToken);

        public Task<OTPLogin> GetById(int Id, CancellationToken cancellationToken);

        public Task<OTPLogin> CreateLoginToken(OTPLogin model, CancellationToken cancellationToken);

        public Task<OTPLogin> UpdateLoginToken(OTPLogin model, CancellationToken cancellationToken);
    }
}