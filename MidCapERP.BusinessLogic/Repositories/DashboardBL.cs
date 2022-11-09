using MidCapERP.BusinessLogic.Interface;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.UnitOfWork;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class DashboardBL : IDashboardBL
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;

        public DashboardBL(IUnitOfWorkDA unitOfWorkDA)
        {
            _unitOfWorkDA = unitOfWorkDA;
        }

        public async Task<int> GetOrderCount(CancellationToken cancellationToken)
        {
            var allOrderData = await _unitOfWorkDA.OrderDA.GetAll(cancellationToken);
            return allOrderData.Count();
        }

        public async Task<int> GetProductCount(CancellationToken cancellationToken)
        {
            var allProductData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return allProductData.Count();
        }

        public async Task<int> GetCustomerCount(CancellationToken cancellationToken)
        {
            var allCustomerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return allCustomerData.Count(x => x.CustomerTypeId == (int)CustomerTypeEnum.Customer);
        }

        public async Task<int> GetInteriorCount(CancellationToken cancellationToken)
        {
            var allCustomerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return allCustomerData.Count(x => x.CustomerTypeId == (int)CustomerTypeEnum.Interior);
        }
    }
}