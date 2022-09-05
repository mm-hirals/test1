using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Customers;
using MidCapERP.Dto.CustomersTypes;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CustomersBL : ICustomersBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public CustomersBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<CustomersResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return _mapper.Map<List<CustomersResponseDto>>(data.ToList());
        }

        public async Task<IEnumerable<CustomersTypesResponseDto>> CustomersTypesGetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerTypesDA.GetAll(cancellationToken);
            return _mapper.Map<List<CustomersTypesResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<CustomersResponseDto>> GetFilterCustomersData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerData = new PagedList<CustomersResponseDto>(_mapper.Map<List<CustomersResponseDto>>(customerAllData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<CustomersResponseDto>(dataTableFilterDto.Draw, customerData.TotalCount, customerData.TotalCount, customerData);
        }

        public async Task<JsonRepsonse<CustomersTypesResponseDto>> GetFilterCustomersTypesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var customerTypesAllData = await _unitOfWorkDA.CustomerTypesDA.GetAll(cancellationToken);
            var customerTypesData = new PagedList<CustomersTypesResponseDto>(_mapper.Map<List<CustomersTypesResponseDto>>(customerTypesAllData).AsQueryable(), dataTableFilterDto);
            return new JsonRepsonse<CustomersTypesResponseDto>(dataTableFilterDto.Draw, customerTypesData.TotalCount, customerTypesData.TotalCount, customerTypesData);
        }

        public async Task<CustomersResponseDto> GetDetailsById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await CustomerGetById(Id, cancellationToken);
            return _mapper.Map<CustomersResponseDto>(data);
        }

        public async Task<CustomersTypesResponseDto> CustomersTypesGetDetailsById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await CustomerTypesGetById(Id, cancellationToken);
            return _mapper.Map<CustomersTypesResponseDto>(data);
        }

        public async Task<CustomersRequestDto> GetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await CustomerGetById(Id, cancellationToken);
            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task<CustomersRequestDto> CreateCustomers(CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var customerToInsert = _mapper.Map<Customers>(model);
            customerToInsert.CustomerId = model.CustomerId;
            customerToInsert.IsDeleted = false;
            customerToInsert.TenantId = _currentUser.TenantId;
            customerToInsert.CreatedBy = _currentUser.UserId;
            customerToInsert.CreatedDate = DateTime.Now;
            customerToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);

            CustomerAddresses catDto = new CustomerAddresses();
            catDto.CustomerId = data.CustomerId;
            catDto.AddressType = model.AddressType;
            catDto.Street1 = model.Street1;
            catDto.Street2 = model.Street2;
            catDto.Landmark = model.Landmark;
            catDto.Area = model.Area;
            catDto.City = model.City;
            catDto.State = model.State;
            catDto.ZipCode = model.ZipCode;
            catDto.IsDefault = model.IsDefault;
            catDto.CreatedDate = DateTime.Now;
            catDto.CreatedUTCDate = DateTime.UtcNow;
            var customerTypeToInsert = _mapper.Map<CustomerAddresses>(catDto);

            await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(catDto, cancellationToken);
            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task<CustomersRequestDto> UpdateCustomers(Int64 Id, CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await CustomerGetById(Id, cancellationToken);
            UpdateCustomers(oldData);
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<CustomersRequestDto>(data);
            return _mappedUser;
        }

        public async Task<CustomersRequestDto> DeleteCustomers(Int64 Id, CancellationToken cancellationToken)
        {
            var customerToInsert = await CustomerGetById(Id, cancellationToken);
            customerToInsert.IsDeleted = true;
            UpdateCustomers(customerToInsert);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, customerToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<CustomersRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private void UpdateCustomers(Customers oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private async Task<Customers> CustomerGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Customer not found");
            }
            return data;
        }

        private async Task<CustomerTypes> CustomerTypesGetById(Int64 Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerTypesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Customer Type not found");
            }
            return data;
        }

        private static void MapToDbObject(CustomersRequestDto model, Customers oldData)
        {
            oldData.CustomerTypeId = model.CustomerTypeId;
            oldData.FirstName = model.FirstName;
            oldData.LastName = model.LastName;
            oldData.EmailId = model.EmailId;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.AltPhoneNumber = model.AltPhoneNumber;
            oldData.GSTNo = model.GSTNo;
            oldData.RefferedBy = model.RefferedBy;
        }

        #endregion PrivateMethods
    }
}