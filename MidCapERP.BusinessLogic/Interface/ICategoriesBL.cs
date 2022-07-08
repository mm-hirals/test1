using MidCapERP.Dto.Categories;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface ICategoriesBL
    {
        public Task<IEnumerable<CategoriesResponseDto>> GetAll(CancellationToken cancellationToken);

        public Task<CategoriesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<CategoriesRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<CategoriesRequestDto> CreateCategory(CategoriesRequestDto model, CancellationToken cancellationToken);

        public Task<CategoriesRequestDto> UpdateCategory(int Id, CategoriesRequestDto model, CancellationToken cancellationToken);

        public Task<CategoriesRequestDto> DeleteCategory(int Id, CancellationToken cancellationToken);
    }
}