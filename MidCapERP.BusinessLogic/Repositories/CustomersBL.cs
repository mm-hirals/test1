using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Customers;
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

        public async Task<IEnumerable<CustomersResponseDto>> GetAllCustomers(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetAllCustomers(cancellationToken);
            return _mapper.Map<List<CustomersResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<CustomersResponseDto>> GetFilterCustomersData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var customerAllData = await _unitOfWorkDA.CustomersDA.GetAllCustomers(cancellationToken);
            var customerData = new PagedList<CustomersResponseDto>(_mapper.Map<List<CustomersResponseDto>>(customerAllData), dataTableFilterDto.Start, dataTableFilterDto.PageSize);
            return new JsonRepsonse<CustomersResponseDto>(dataTableFilterDto.Draw, customerData.TotalCount, customerData.TotalCount, customerData);
        }

        public async Task<CustomersResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await CustomerGetById(Id, cancellationToken);
            return _mapper.Map<CustomersResponseDto>(data);
        }

        public async Task<CustomersRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await CustomerGetById(Id, cancellationToken);
            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task<CustomersRequestDto> CreateCustomers(CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var customerToInsert = _mapper.Map<Customers>(model);
            customerToInsert.IsDeleted = false;
            customerToInsert.TenantId = _currentUser.TenantId;
            customerToInsert.CreatedBy = _currentUser.UserId;
            customerToInsert.CreatedDate = DateTime.Now;
            customerToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<CustomersRequestDto>(data);
            return _mappedUser;
        }

        public async Task<CustomersRequestDto> UpdateCustomers(int Id, CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await CustomerGetById(Id, cancellationToken);
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<CustomersRequestDto>(data);
            return _mappedUser;
        }

        public async Task<CustomersRequestDto> DeleteCustomers(int Id, CancellationToken cancellationToken)
        {
            var customerToInsert = await CustomerGetById(Id, cancellationToken);
            customerToInsert.IsDeleted = true;
            customerToInsert.UpdatedBy = _currentUser.UserId;
            customerToInsert.UpdatedDate = DateTime.Now;
            customerToInsert.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, customerToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<CustomersRequestDto>(data);
            return _mappedUser;
        }

        #region PrivateMethods

        private async Task<Customers> CustomerGetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomersDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Customer not found");
            }
            return data;
        }

        private static void MapToDbObject(CustomersRequestDto model, Customers oldData)
        {
            oldData.CustomerName = model.CustomerName;
            oldData.EmailId = model.EmailId;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.AltPhoneNumber = model.AltPhoneNumber;
            oldData.BillingStreet1 = model.BillingStreet1;
            oldData.BillingStreet2 = model.BillingStreet2;
            oldData.BillingLandmark = model.BillingLandmark;
            oldData.BillingArea = model.BillingArea;
            oldData.BillingCity = model.BillingCity;
            oldData.BillingState = model.BillingState;
            oldData.BillingZipCode = model.BillingZipCode;
            oldData.ShippingStreet1 = model.ShippingStreet1;
            oldData.ShippingStreet2 = model.ShippingStreet2;
            oldData.ShippingLandmark = model.ShippingLandmark;
            oldData.ShippingArea = model.ShippingArea;
            oldData.ShippingCity = model.ShippingCity;
            oldData.ShippingState = model.ShippingState;
            oldData.ShippingZipCode = model.ShippingZipCode;
        }

        #endregion PrivateMethods
    }
}