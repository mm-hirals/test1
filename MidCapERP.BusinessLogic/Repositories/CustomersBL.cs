using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Customers;

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
            var DataToReturn = _mapper.Map<List<CustomersResponseDto>>(data.ToList());
            return DataToReturn;
        }

        public async Task<CustomersResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = CustomerGetById(Id, cancellationToken).Result;
            if (data == null)
            {
                throw new Exception("Customer not found");
            }
            return _mapper.Map<CustomersResponseDto>(data);
        }

        public async Task<CustomersRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = CustomerGetById(Id, cancellationToken).Result;
            if (data == null)
            {
                throw new Exception("Customer not found");
            }
            return _mapper.Map<CustomersRequestDto>(data);
        }

        public async Task<CustomersRequestDto> CreateCustomers(CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var customerToInsert = _mapper.Map<Customers>(model);
            customerToInsert.IsDeleted = false;
            customerToInsert.CreatedBy = _currentUser.UserId;
            customerToInsert.TenantId = _currentUser.TenantId;
            customerToInsert.CreatedDate = DateTime.Now;
            customerToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CustomersDA.CreateCustomers(customerToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<CustomersRequestDto>(data);
            return _mappedUser;
        }

        public async Task<CustomersRequestDto> UpdateCustomers(int Id, CustomersRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = CustomerGetById(Id, cancellationToken).Result;
            if (oldData == null)
            {
                throw new Exception("Customer not found");
            }
            model.TenantId = _currentUser.TenantId;
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CustomersDA.UpdateCustomers(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<CustomersRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(CustomersRequestDto model, Customers oldData)
        {
            oldData.CustomerName = model.CustomerName;
            oldData.BillingAddress = model.BillingAddress;
            oldData.ShippingAddress = model.ShippingAddress;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.TenantId = model.TenantId;
            oldData.IsDeleted = model.IsDeleted;
        }

        public async Task<CustomersRequestDto> DeleteCustomers(int Id, CancellationToken cancellationToken)
        {
            var customerToInsert = CustomerGetById(Id, cancellationToken).Result;
            if (customerToInsert == null)
            {
                throw new Exception("Customer not found");
            }
            customerToInsert.TenantId = _currentUser.TenantId;
            customerToInsert.IsDeleted = true;
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

        #endregion PrivateMethods
    }
}
