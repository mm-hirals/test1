using MidCapERP.Dto.Lookups;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ILookupsBL
    {
        public Task<IEnumerable<LookupsResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<LookupsResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<LookupsRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<LookupsRequestDto> CreateLookup(LookupsRequestDto model, CancellationToken cancellationToken);

        public Task<LookupsRequestDto> UpdateLookup(int Id, LookupsRequestDto model, CancellationToken cancellationToken);

        public Task<LookupsRequestDto> DeleteLookup(int Id, CancellationToken cancellationToken);
    }
}
