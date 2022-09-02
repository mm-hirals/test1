using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IFrameDA
    {
        public Task<IQueryable<Frames>> GetAll(CancellationToken cancellationToken);

        public Task<Frames> GetById(int Id, CancellationToken cancellationToken);

        public Task<Frames> CreateFrame(Frames model, CancellationToken cancellationToken);

        public Task<Frames> UpdateFrame(int Id, Frames model, CancellationToken cancellationToken);

        public Task<Frames> DeleteFrame(int Id, CancellationToken cancellationToken);
    }
}