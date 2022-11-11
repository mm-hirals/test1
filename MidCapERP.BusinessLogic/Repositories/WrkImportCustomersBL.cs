using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.WrkImportCustomers;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class WrkImportCustomersBL : IWrkImportCustomersBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public WrkImportCustomersBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<List<WrkImportCustomersDto>> CreateWrkCustomer(List<WrkImportCustomersDto> model, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWorkDA.BeginTransactionAsync();
                foreach (var item in model)
                {
                    var wrkCustomer = _mapper.Map<WrkImportCustomers>(item);
                    wrkCustomer.CreatedBy = _currentUser.UserId;
                    wrkCustomer.CreatedDate = DateTime.Now;
                    wrkCustomer.CreatedUTCDate = DateTime.UtcNow;
                    await _unitOfWorkDA.WrkImportCustomersDA.Create(wrkCustomer, cancellationToken);
                }
                await _unitOfWorkDA.CommitTransactionAsync();
                return _mapper.Map<List<WrkImportCustomersDto>>(model);
            }
            catch (Exception)
            {
                await _unitOfWorkDA.rollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IEnumerable<WrkImportCustomersDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.WrkImportCustomersDA.GetAll(cancellationToken);
            return _mapper.Map<List<WrkImportCustomersDto>>(data.ToList());
        }

        public async Task<WrkImportCustomersDto> GetById(long WrkCustomerID, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.WrkImportCustomersDA.GetById(WrkCustomerID, cancellationToken);
            return _mapper.Map<WrkImportCustomersDto>(data);
        }
    }
}