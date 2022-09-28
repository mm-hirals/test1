using MidCapERP.DataAccess.Generic;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.DataAccess.Repositories
{
    public class FrameDA : IFrameDA
    {
        private readonly ISqlRepository<Frames> _frame;
        private readonly CurrentUser _currentUser;

        public FrameDA(ISqlRepository<Frames> frame, CurrentUser currentUser)
        {
            _frame = frame;
            _currentUser = currentUser;
        }

        public async Task<IQueryable<Frames>> GetAll(CancellationToken cancellationToken)
        {
            return await _frame.GetAsync(cancellationToken, x => x.IsDeleted == false && x.TenantId == _currentUser.TenantId);
        }

        public async Task<Frames> GetById(int Id, CancellationToken cancellationToken)
        {
            return await _frame.GetByIdAsync(Id, cancellationToken);
        }

        public async Task<Frames> CreateFrame(Frames model, CancellationToken cancellationToken)
        {
            return await _frame.InsertAsync(model, cancellationToken);
        }

        public async Task<Frames> UpdateFrame(int Id, Frames model, CancellationToken cancellationToken)
        {
            return await _frame.UpdateAsync(model, cancellationToken);
        }

        public async Task<Frames> DeleteFrame(int Id, CancellationToken cancellationToken)
        {
            var entity = await _frame.GetByIdAsync(Id, cancellationToken);
            if (entity != null)
            {
                return await _frame.UpdateAsync(entity, cancellationToken);
            }
            return entity;
        }
    }
}