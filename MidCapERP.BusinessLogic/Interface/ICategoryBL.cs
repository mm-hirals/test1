using MidCapERP.Dto.Category;
using MidCapERP.Dto.DataGrid;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICategoryBL
    {
        public Task<IEnumerable<CategoryResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<CategoryResponseDto>> GetFilterCategoryData(CategoryDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<CategoryRequestDto> GetById(long Id, CancellationToken cancellationToken);

        public Task<CategoryRequestDto> CreateCategory(CategoryRequestDto model, CancellationToken cancellationToken);

        public Task<CategoryRequestDto> UpdateCategory(long Id, CategoryRequestDto model, CancellationToken cancellationToken);

        public Task<CategoryRequestDto> DeleteCategory(int Id, CancellationToken cancellationToken);

        public Task<bool> ValidateCategoryName(CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken);
    }
}