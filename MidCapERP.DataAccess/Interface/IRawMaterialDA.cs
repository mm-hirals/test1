using MidCapERP.DataEntities.Models;


namespace MidCapERP.DataAccess.Interface
{
    public interface IRawMaterialDA
    {
        public Task<IQueryable<RawMaterial>> GetAll(CancellationToken cancellationToken);

        public Task<RawMaterial> GetById(int Id, CancellationToken cancellationToken);

        public Task<RawMaterial> CreateRawMaterial(RawMaterial model, CancellationToken cancellationToken);

        public Task<RawMaterial> UpdateRawMaterial(int Id, RawMaterial model, CancellationToken cancellationToken);

        public Task<RawMaterial> DeleteRawMaterial(int Id, CancellationToken cancellationToken);
    }
}
