using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.CustomerVisit;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CustomerVisitsBL : ICustomerVisitsBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private CurrentUser _currentUser;

        public CustomerVisitsBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<CustomerVisitRequestDto> CustomerVisitAPI(CustomerVisitRequestDto model, CancellationToken cancellationToken)
        {
            var allCustomers = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            var customerExists = allCustomers.FirstOrDefault(p => p.CustomerId == model.CustomerId);
            if (customerExists != null)
            {
                var customerVisitsToInsert = _mapper.Map<CustomerVisits>(model);
                customerVisitsToInsert.CustomerId = model.CustomerId;
                customerVisitsToInsert.Comment = model.Comment;
                customerVisitsToInsert.CreatedBy = _currentUser.UserId;
                customerVisitsToInsert.CreatedDate = DateTime.Now;
                customerVisitsToInsert.CreatedUTCDate = DateTime.UtcNow;
                var data = await _unitOfWorkDA.CustomerVisitsDA.CreateCustomerVisits(customerVisitsToInsert, cancellationToken);
                return _mapper.Map<CustomerVisitRequestDto>(data);
            }
            else
                throw new Exception("Customer doesn't exist.");
        }

        public async Task<IEnumerable<CustomerVisitResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerVisitsDA.GetAll(cancellationToken);
            return _mapper.Map<List<CustomerVisitResponseDto>>(data.ToList());
        }

        public async Task<IEnumerable<CustomerVisitResponseDto>> GetById(long customerId, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerVisitsDA.GetAll(cancellationToken);
            data = data.Where(x => x.CustomerId == customerId);
            return _mapper.Map<List<CustomerVisitResponseDto>>(data.ToList());
        }
    }
}