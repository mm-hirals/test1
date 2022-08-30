using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.Dto.CustomerAddresses;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CustomerAddressesBL : ICustomerAddressesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;

        public CustomerAddressesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerAddressesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            return _mapper.Map<List<CustomerAddressesResponseDto>>(data.ToList());
        }

        public async Task<JsonRepsonse<CustomerAddressesResponseDto>> GetFilterCustomerAddressesData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var customerAddressesAllData = await _unitOfWorkDA.CustomerAddressesDA.GetAll(cancellationToken);
            var customerAddressesResponseData = (from x in customerAddressesAllData
                                                 select new CustomerAddressesResponseDto()
                                                 {
                                                     CustAddressId = x.CustAddressId,
                                                     AddressTypeId = x.AddressTypeId,
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

        public async Task<CustomerAddressesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await GetDetailsById(Id, cancellationToken);
            return _mapper.Map<CustomerAddressesResponseDto>(data);
        }
    }
}