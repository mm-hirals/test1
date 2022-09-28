using MidCapERP.Dto.Category;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICategoryBL
    {
        public Task<IEnumerable<CategoryResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<CategoryResponseDto> GetCategorySearchByCategoryName(string searchName, CancellationToken cancellation);

        public Task<JsonRepsonse<CategoryResponseDto>> GetFilterCategoryData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<CategoryResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<CategoryRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<CategoryRequestDto> CreateCategory(CategoryRequestDto model, CancellationToken cancellationToken);

        public Task<CategoryRequestDto> UpdateCategory(int Id, CategoryRequestDto model, CancellationToken cancellationToken);

        public Task<CategoryRequestDto> DeleteCategory(int Id, CancellationToken cancellationToken);
    }
}