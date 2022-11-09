using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.InteriorAddresses;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class InteriorAddressesBL : IInteriorAddressesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public InteriorAddressesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<InteriorAddressesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            return _mapper.Map<List<InteriorAddressesResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<InteriorAddressesResponseDto>> GetFilterInteriorAddressesData(InteriorAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var interiorAddressesAllData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var interior = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var interiorAddressesResponseData = (from x in interiorAddressesAllData
                                                 join y in interior on x.CustomerId equals y.CustomerId
                                                 where x.CustomerId == dataTableFilterDto.customerId
                                                 select new InteriorAddressesResponseDto()
                                                 {
                                                     CustomerAddressId = x.CustomerAddressId,
                                                     CustomerId = x.CustomerId,
                                                     AddressType = x.AddressType,
                                                     Street1 = x.Street1,
                                                     Street2 = x.Street2,
                                                     Landmark = x.Landmark,
                                                     Area = x.Area,
                                                     City = x.City,
                                                     State = x.State,
                                                     ZipCode = x.ZipCode,
                                                     IsDefault = x.IsDefault,
                                                 }).AsQueryable();
            var interiorAddressesData = new PagedList<InteriorAddressesResponseDto>(interiorAddressesResponseData, dataTableFilterDto);
            return new JsonRepsonse<InteriorAddressesResponseDto>(dataTableFilterDto.Draw, interiorAddressesData.TotalCount, interiorAddressesData.TotalCount, interiorAddressesData);
        }

        public async Task<InteriorAddressesRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await InteriorAddressesGetById(Id, cancellationToken);
            return _mapper.Map<InteriorAddressesRequestDto>(data);
        }

        public async Task<IEnumerable<InteriorAddressesResponseDto>> GetInteriorById(Int64 Id, CancellationToken cancellationToken)
        {
            var interiorAddress = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var interiorAddressData = interiorAddress.Where(x => x.CustomerId == Id);
            return _mapper.Map<List<InteriorAddressesResponseDto>>(interiorAddressData);
        }

        public async Task<InteriorAddressesRequestDto> CreateInteriorAddresses(InteriorAddressesRequestDto model, CancellationToken cancellationToken)
        {
            if (model.IsDefault)
            {
                var defualtAddress = await GetInteriorDefualtAddress(model.CustomerId, cancellationToken);
                if (defualtAddress != null)
                {
                    defualtAddress.IsDefault = false;
                    await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(defualtAddress.CustomerAddressId, defualtAddress, cancellationToken);
                }
            }
            var interiorAddresses = _mapper.Map<CustomerAddresses>(model);
            interiorAddresses.AddressType = model.AddressType != "" ? model.AddressType : "Home";
            interiorAddresses.Street1 = model.Street1;
            interiorAddresses.Street2 = model.Street2 != null ? model.Street2 : String.Empty;
            interiorAddresses.Area = model.Area;
            interiorAddresses.Landmark = model.Landmark != null ? model.Landmark : String.Empty;
            interiorAddresses.City = model.City;
            interiorAddresses.State = model.State;
            interiorAddresses.ZipCode = model.ZipCode;
            interiorAddresses.IsDeleted = false;
            interiorAddresses.CreatedBy = _currentUser.UserId;
            interiorAddresses.CreatedDate = DateTime.Now;
            interiorAddresses.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(interiorAddresses, cancellationToken);
            return _mapper.Map<InteriorAddressesRequestDto>(data);
        }

        public async Task<InteriorAddressesRequestDto> UpdateInteriorAddresses(Int64 Id, InteriorAddressesRequestDto model, CancellationToken cancellationToken)
        {
            if (model.IsDefault)
            {
                var defualtAddress = await GetInteriorDefualtAddress(model.CustomerId, cancellationToken);
                if (defualtAddress != null)
                {
                    defualtAddress.IsDefault = false;
                    await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(defualtAddress.CustomerAddressId, defualtAddress, cancellationToken);
                }
            }
            var oldData = await InteriorAddressesGetById(Id, cancellationToken);
            UpdateInteriorAddresses(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(Id, oldData, cancellationToken);
            return _mapper.Map<InteriorAddressesRequestDto>(data);
        }

        public async Task<InteriorAddressesRequestDto> DeleteInteriorAddresses(Int64 Id, CancellationToken cancellationToken)
        {
            var interiorAddressToInsert = await InteriorAddressesGetById(Id, cancellationToken);
            interiorAddressToInsert.IsDeleted = true;
            UpdateInteriorAddresses(interiorAddressToInsert);
            var data = await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(Id, interiorAddressToInsert, cancellationToken);
            return _mapper.Map<InteriorAddressesRequestDto>(data);
        }

        #region PrivateMethods

        private void UpdateInteriorAddresses(CustomerAddresses oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private async Task<CustomerAddresses> InteriorAddressesGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerAddressesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Interior Address not found");
            }
            return data;
        }

        private static void MapToDbObject(InteriorAddressesRequestDto model, CustomerAddresses oldData)
        {
            oldData.CustomerId = model.CustomerId;
            oldData.AddressType = model.AddressType;
            oldData.Street1 = model.Street1 != null ? model.Street1 : String.Empty;
            oldData.Street2 = model.Street2 != null ? model.Street2 : String.Empty;
            oldData.Landmark = model.Landmark != null ? model.Landmark : String.Empty;
            oldData.Area = model.Area != null ? model.Area : String.Empty;
            oldData.City = model.City != null ? model.City : String.Empty;
            oldData.State = model.State != null ? model.State : String.Empty;
            oldData.ZipCode = model.ZipCode != null ? model.ZipCode : String.Empty;
            oldData.IsDefault = model.IsDefault;
        }

        private async Task<CustomerAddresses?> GetInteriorDefualtAddress(Int64 Id, CancellationToken cancellationToken)
        {
            var interiorAddressesAllData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var interiorHaveDefualtAddress = interiorAddressesAllData.FirstOrDefault(x => x.CustomerId == Id && x.IsDefault == true);
            if (interiorHaveDefualtAddress != null)
            {
                return interiorHaveDefualtAddress;
            }
            return null;
        }

        #endregion PrivateMethods
    }
}