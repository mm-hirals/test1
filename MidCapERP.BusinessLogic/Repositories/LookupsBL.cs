using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Lookups;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class LookupsBL : ILookupsBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public LookupsBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<LookupsResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var DataToReturn = _mapper.Map<List<LookupsResponseDto>>(data.ToList());
            return DataToReturn;
        }

        public async Task<LookupsResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.LookupsDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Lookup not found");
            }
            return _mapper.Map<LookupsResponseDto>(data);
        }

        public async Task<LookupsRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var lookUpDataById = LookupGetById(Id, cancellationToken).Result;
            return _mapper.Map<LookupsRequestDto>(lookUpDataById);
        }

        public async Task<LookupsRequestDto> CreateLookup(LookupsRequestDto model, CancellationToken cancellationToken)
        {
            var lookupToInsert = _mapper.Map<Lookups>(model);
            lookupToInsert.IsDeleted = false;
            lookupToInsert.CreatedBy = _currentUser.UserId;
            lookupToInsert.TenantId = _currentUser.TenantId;
            lookupToInsert.CreatedDate = DateTime.Now;
            lookupToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupsDA.CreateLookup(lookupToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<LookupsRequestDto>(data);
            return _mappedUser;
        }

        public async Task<LookupsRequestDto> UpdateLookup(int Id, LookupsRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = LookupGetById(Id, cancellationToken).Result;
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.LookupsDA.UpdateLookup(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<LookupsRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(LookupsRequestDto model, Lookups oldData)
        {
            oldData.LookupName = model.LookupName;
            oldData.IsDeleted = model.IsDeleted;
        }

        public async Task<LookupsRequestDto> DeleteLookup(int Id, CancellationToken cancellationToken)
        {
            var lookupToUpdate = LookupGetById(Id, cancellationToken).Result;
            lookupToUpdate.IsDeleted = true;
            lookupToUpdate.UpdatedDate = DateTime.Now;
            lookupToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupsDA.UpdateLookup(Id, lookupToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<LookupsRequestDto>(data);
            return _mappedUser;
        }

        #region otherMethod

        private async Task<Lookups> LookupGetById(int Id, CancellationToken cancellationToken)
        {
            var lookupDataById = await _unitOfWorkDA.LookupsDA.GetById(Id, cancellationToken);
            if (lookupDataById == null)
            {
                throw new Exception("Lookup not found");
            }
            return lookupDataById;
        }

        #endregion otherMethod
    }
}