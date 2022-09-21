using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.Dto.Constants;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class DashboardBL : IDashboardBL
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;

        public DashboardBL(IUnitOfWorkDA unitOfWorkDA)
        {
            _unitOfWorkDA = unitOfWorkDA;
        }

        public async Task<int> GetProductCount(CancellationToken cancellationToken)
        {
            var allProductData = await _unitOfWorkDA.ProductDA.GetAll(cancellationToken);
            return allProductData.Count();
        }

        public async Task<int> GetCustomerCount(CancellationToken cancellationToken)
        {
            var allCustomerData = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
            return allCustomerData.Count();
        }

        public async Task<int> GetCategoriesCount(CancellationToken cancellationToken)
        {
            var allCategoriesData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var categoriesCount = allCategoriesData.Where(x => x.LookupId == (int)MasterPagesEnum.Category).Count();

            return categoriesCount;
        }
    }
}