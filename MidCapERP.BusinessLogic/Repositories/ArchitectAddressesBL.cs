using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class ArchitectAddressesBL : IArchitectAddressesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public ArchitectAddressesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<CustomerAddressesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            return _mapper.Map<List<CustomerAddressesResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<CustomerAddressesResponseDto>> GetFilterArchitectAddressesData(CustomerAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var architectAddressesAllData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var architect = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var architectAddressesResponseData = (from x in architectAddressesAllData
                                                  join y in architect on x.CustomerId equals y.CustomerId
                                                  where x.CustomerId == dataTableFilterDto.customerId
                                                  select new CustomerAddressesResponseDto()
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
            var architectAddressesData = new PagedList<CustomerAddressesResponseDto>(architectAddressesResponseData, dataTableFilterDto);
            return new JsonRepsonse<CustomerAddressesResponseDto>(dataTableFilterDto.Draw, architectAddressesData.TotalCount, architectAddressesData.TotalCount, architectAddressesData);
        }

        public async Task<CustomerAddressesRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await ArchitectAddressesGetById(Id, cancellationToken);
            return _mapper.Map<CustomerAddressesRequestDto>(data);
        }

        public async Task<IEnumerable<CustomerAddressesResponseDto>> GetArchitectById(Int64 Id, CancellationToken cancellationToken)
        {
            var architectAddress = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var architectAddressData = architectAddress.Where(x => x.CustomerId == Id);
            return _mapper.Map<List<CustomerAddressesResponseDto>>(architectAddressData);
        }

        public async Task<CustomerAddressesRequestDto> CreateArchitectAddresses(CustomerAddressesRequestDto model, CancellationToken cancellationToken)
        {
            if (model.IsDefault)
            {
                var defualtAddress = await GetArchitectDefualtAddress(model.CustomerId, cancellationToken);
                if (defualtAddress != null)
                {
                    defualtAddress.IsDefault = false;
                    await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(defualtAddress.CustomerAddressId, defualtAddress, cancellationToken);
                }
            }
            var architectAddresses = _mapper.Map<CustomerAddresses>(model);
            architectAddresses.IsDeleted = false;
            architectAddresses.CreatedBy = _currentUser.UserId;
            architectAddresses.CreatedDate = DateTime.Now;
            architectAddresses.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(architectAddresses, cancellationToken);
            return _mapper.Map<CustomerAddressesRequestDto>(data);
        }

        public async Task<CustomerAddressesRequestDto> UpdateArchitectAddresses(Int64 Id, CustomerAddressesRequestDto model, CancellationToken cancellationToken)
        {
            if (model.IsDefault)
            {
                var defualtAddress = await GetArchitectDefualtAddress(model.CustomerId, cancellationToken);
                if (defualtAddress != null)
                {
                    defualtAddress.IsDefault = false;
                    await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(defualtAddress.CustomerAddressId, defualtAddress, cancellationToken);
                }
            }
            var oldData = await ArchitectAddressesGetById(Id, cancellationToken);
            UpdateArchitectAddresses(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(Id, oldData, cancellationToken);
            return _mapper.Map<CustomerAddressesRequestDto>(data);
        }

        public async Task<CustomerAddressesRequestDto> DeleteArchitectAddresses(Int64 Id, CancellationToken cancellationToken)
        {
            var architectAddressToInsert = await ArchitectAddressesGetById(Id, cancellationToken);
            architectAddressToInsert.IsDeleted = true;
            UpdateArchitectAddresses(architectAddressToInsert);
            var data = await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(Id, architectAddressToInsert, cancellationToken);
            return _mapper.Map<CustomerAddressesRequestDto>(data);
        }

        #region PrivateMethods

        private void UpdateArchitectAddresses(CustomerAddresses oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private async Task<CustomerAddresses> ArchitectAddressesGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerAddressesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Architect Address not found");
            }
            return data;
        }

        private static void MapToDbObject(CustomerAddressesRequestDto model, CustomerAddresses oldData)
        {
            oldData.CustomerId = model.CustomerId;
            oldData.AddressType = model.AddressType;
            oldData.Street1 = model.Street1;
            oldData.Street2 = model.Street2;
            oldData.Landmark = model.Landmark;
            oldData.Area = model.Area;
            oldData.City = model.City;
            oldData.State = model.State;
            oldData.ZipCode = model.ZipCode;
            oldData.IsDefault = model.IsDefault;
        }

        private async Task<CustomerAddresses?> GetArchitectDefualtAddress(Int64 Id, CancellationToken cancellationToken)
        {
            var architectAddressesAllData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var architectHaveDefualtAddress = architectAddressesAllData.FirstOrDefault(x => x.CustomerId == Id && x.IsDefault == true);
            if (architectHaveDefualtAddress != null)
            {
                return architectHaveDefualtAddress;
            }
            return null;
        }

        #endregion PrivateMethods
    }
}