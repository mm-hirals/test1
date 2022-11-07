using MidCapERP.Dto.Company;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICompanyBL
    {
        public Task<IEnumerable<CompanyResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<CompanyResponseDto>> GetFilterCompanyData(CompanyDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<CompanyResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<CompanyRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<CompanyRequestDto> CreateCompany(CompanyRequestDto model, CancellationToken cancellationToken);

        public Task<CompanyRequestDto> UpdateCompany(int Id, CompanyRequestDto model, CancellationToken cancellationToken);

        public Task<CompanyRequestDto> DeleteCompany(int Id, CancellationToken cancellationToken);

        public Task<bool> ValidateCompanyName(CompanyRequestDto comRequestDto, CancellationToken cancellationToken);
    }
}