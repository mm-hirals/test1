﻿using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CustomerAddressesBL : ICustomerAddressesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public CustomerAddressesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
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

        public async Task<JsonRepsonse<CustomerAddressesResponseDto>> GetFilterCustomerAddressesData(CustomerAddressDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var customerAddressesAllData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var customer = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerAddressesResponseData = (from x in customerAddressesAllData
                                                 join y in customer on x.CustomerId equals y.CustomerId
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
            var customerAddressesData = new PagedList<CustomerAddressesResponseDto>(customerAddressesResponseData, dataTableFilterDto);
            return new JsonRepsonse<CustomerAddressesResponseDto>(dataTableFilterDto.Draw, customerAddressesData.TotalCount, customerAddressesData.TotalCount, customerAddressesData);
        }

        public async Task<CustomerAddressesResponseDto> GetDetailsById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await GetDetailsById(Id, cancellationToken);
            return _mapper.Map<CustomerAddressesResponseDto>(data);
        }

        public async Task<CustomerAddressesRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await CustomerAddressesGetById(Id, cancellationToken);
            return _mapper.Map<CustomerAddressesRequestDto>(data);
        }

        public async Task<IEnumerable<CustomerAddressesResponseDto>> GetCustomerById(Int64 Id, CancellationToken cancellationToken)
        {
            var customerAddress = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var customerAddressData = customerAddress.Where(x => x.CustomerId == Id);
            return _mapper.Map<List<CustomerAddressesResponseDto>>(customerAddressData);
        }

        public async Task<CustomerAddressesResponseDto> GetCustomerAddressById(Int64 Id, CancellationToken cancellationToken)
        {
            var customerAddressData = await CustomerAddressesGetById(Id, cancellationToken);
            return _mapper.Map<CustomerAddressesResponseDto>(customerAddressData);
        }

        public async Task<CustomerAddressesRequestDto> CreateCustomerAddresses(CustomerAddressesRequestDto model, CancellationToken cancellationToken)
        {
            if (model.IsDefault)
            {
                var defualtAddress = await GetCustomerDefualtAddress(model.CustomerId, cancellationToken);
                if (defualtAddress != null)
                {
                    defualtAddress.IsDefault = false;
                    await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(defualtAddress.CustomerAddressId, defualtAddress, cancellationToken);
                }
            }
            var customerAddresses = _mapper.Map<CustomerAddresses>(model);
            customerAddresses.AddressType = model.AddressType != "" ? model.AddressType : "Home";
            customerAddresses.Street1 = model.Street1 != null ? model.Street1 : String.Empty;
            customerAddresses.Street2 = model.Street2 != null ? model.Street2 : String.Empty;
            customerAddresses.Area = model.Area != null ? model.Area : String.Empty;
            customerAddresses.Landmark = model.Landmark != null ? model.Landmark : String.Empty;
            customerAddresses.City = model.City != null ? model.City : String.Empty;
            customerAddresses.State = model.State != null ? model.State : String.Empty;
            customerAddresses.ZipCode = model.ZipCode != null ? model.ZipCode : String.Empty;
            customerAddresses.IsDeleted = false;
            customerAddresses.CreatedBy = _currentUser.UserId;
            customerAddresses.CreatedDate = DateTime.Now;
            customerAddresses.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(customerAddresses, cancellationToken);
            return _mapper.Map<CustomerAddressesRequestDto>(data);
        }

        public async Task<CustomerAddressesApiRequestDto> CreateCustomerApiAddresses(CustomerAddressesApiRequestDto model, CancellationToken cancellationToken)
        {
            var customerData = await _unitOfWorkDA.CustomersDA.GetById(model.CustomerId, cancellationToken);
            if (customerData == null)
            {
                throw new Exception("Customer not found");
            }
            var customerAddresses = _mapper.Map<CustomerAddresses>(model);
            //customerAddresses.AddressType = model.AddressType;
            //customerAddresses.Street1 = model.Street1;
            //customerAddresses.Street2 = model.Street2 != null ? model.Street2 : String.Empty;
            //customerAddresses.Landmark = model.Landmark != null ? model.Landmark : String.Empty;
            //customerAddresses.Area = model.Area;
            //customerAddresses.City = model.City;
            //customerAddresses.State = model.State;
            //customerAddresses.ZipCode = model.ZipCode;
            //customerAddresses.IsDefault = model.IsDefault;
            customerAddresses.IsDeleted = false;
            customerAddresses.CreatedBy = _currentUser.UserId;
            customerAddresses.CreatedDate = DateTime.Now;
            customerAddresses.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(customerAddresses, cancellationToken);
            return _mapper.Map<CustomerAddressesApiRequestDto>(data);
        }

        public async Task<CustomerAddressesRequestDto> UpdateCustomerAddresses(Int64 Id, CustomerAddressesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await CustomerAddressesGetById(Id, cancellationToken);
            UpdateCustomerAddresses(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(Id, oldData, cancellationToken);
            return _mapper.Map<CustomerAddressesRequestDto>(data);
        }

        public async Task<CustomerAddressesApiRequestDto> UpdateCustomerApiAddresses(Int64 Id, CustomerAddressesApiRequestDto model, CancellationToken cancellationToken)
        {
            if (model.IsDefault)
            {
                var defualtAddress = await GetCustomerDefualtAddress(model.CustomerId, cancellationToken);
                if (defualtAddress != null)
                {
                    defualtAddress.IsDefault = false;
                    await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(defualtAddress.CustomerAddressId, defualtAddress, cancellationToken);
                }
            }
            var oldData = await CustomerAddressesGetById(Id, cancellationToken);
            UpdateCustomerAddresses(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(Id, oldData, cancellationToken);
            return _mapper.Map<CustomerAddressesApiRequestDto>(data);
        }

        public async Task<CustomerAddresses?> GetCustomerDefualtAddress(Int64 Id, CancellationToken cancellationToken)
        {
            var customerAddressesAllData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var customerHaveDefualtAddress = customerAddressesAllData.FirstOrDefault(x => x.CustomerId == Id && x.IsDefault == true);
            if (customerHaveDefualtAddress != null)
            {
                return customerHaveDefualtAddress;
            }
            return null;
        }

        public async Task<CustomerAddressesRequestDto> DeleteCustomerAddresses(Int64 Id, CancellationToken cancellationToken)
        {
            var customerAddressToInsert = await CustomerAddressesGetById(Id, cancellationToken);
            customerAddressToInsert.IsDeleted = true;
            UpdateCustomerAddresses(customerAddressToInsert);
            var data = await _unitOfWorkDA.CustomerAddressesDA.UpdateCustomerAddress(Id, customerAddressToInsert, cancellationToken);
            return _mapper.Map<CustomerAddressesRequestDto>(data);
        }

        #region PrivateMethods

        private void UpdateCustomerAddresses(CustomerAddresses oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private async Task<CustomerAddresses> CustomerAddressesGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerAddressesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Customer Address not found");
            }
            return data;
        }

        private static void MapToDbObject(CustomerAddressesRequestDto model, CustomerAddresses oldData)
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

        private static void MapToDbObject(CustomerAddressesApiRequestDto model, CustomerAddresses oldData)
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

        #endregion PrivateMethods
    }
}