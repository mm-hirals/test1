using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.Categories;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CategoriesBL : ICategoriesBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;

        public CategoriesBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriesResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CategoriesDA.GetAll(cancellationToken);
            var DataToReturn = _mapper.Map<List<CategoriesResponseDto>>(data.ToList());
            return DataToReturn;
        }

        public async Task<CategoriesResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CategoriesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Category not found");
            }
            return _mapper.Map<CategoriesResponseDto>(data);
        }

        public async Task<CategoriesRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.CategoriesDA.GetById(Id, cancellationToken);
            if (data == null)
            {
                throw new Exception("Category not found");
            }
            return _mapper.Map<CategoriesRequestDto>(data);
        }

        public async Task<CategoriesRequestDto> CreateCategory(CategoriesRequestDto model, CancellationToken cancellationToken)
        {
            var categoryToInsert = _mapper.Map<Categories>(model);
            categoryToInsert.IsDeleted = true;
            var data = await _unitOfWorkDA.CategoriesDA.CreateCategory(categoryToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<CategoriesRequestDto>(data);
            return _mappedUser;
        }

        public async Task<CategoriesRequestDto> UpdateCategory(int Id, CategoriesRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await _unitOfWorkDA.CategoriesDA.GetById(Id, cancellationToken);
            if (oldData == null)
            {
                throw new Exception("Category not found");
            }
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
            MapToDbObject(model, oldData);
            var data = await _unitOfWorkDA.CategoriesDA.UpdateCategory(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<CategoriesRequestDto>(data);
            return _mappedUser;
        }

        private static void MapToDbObject(CategoriesRequestDto model, Categories oldData)
        {
            oldData.CategoryName = model.CategoryName;
        }

        public async Task<CategoriesRequestDto> DeleteCategory(int Id, CancellationToken cancellationToken)
        {
            var categoryToUpdate = await _unitOfWorkDA.CategoriesDA.GetById(Id, cancellationToken);
            if (categoryToUpdate == null)
            {
                throw new Exception("Category not found");
            }
            categoryToUpdate.IsDeleted = false;
            categoryToUpdate.UpdatedDate = DateTime.Now;
            categoryToUpdate.UpdatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.CategoriesDA.UpdateCategory(Id, categoryToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<CategoriesRequestDto>(data);
            return _mappedUser;
        }
    }
}