using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Category;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class CategoryBL : ICategoryBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public CategoryBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var data = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            return _mapper.Map<List<CategoryResponseDto>>(data.Where(x => x.LookupId == (int)MasterPagesEnum.Category).ToList());
        }

        public async Task<JsonRepsonse<CategoryResponseDto>> GetFilterCategoryData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var categoryAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var lookupsAllData = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var categoryResponseData = (from x in categoryAllData
                                        join y in lookupsAllData
                                        on new { x.LookupId } equals new { y.LookupId }
                                        where x.LookupId == (int)MasterPagesEnum.Category
                                        select new CategoryResponseDto()
                                        {
                                            LookupValueId = x.LookupValueId,
                                            LookupValueName = x.LookupValueName,
                                            LookupId = x.LookupId,
                                            LookupName = y.LookupName,
                                            IsDeleted = x.IsDeleted,
                                            CreatedBy = x.CreatedBy,
                                            CreatedDate = x.CreatedDate,
                                            CreatedUTCDate = x.CreatedUTCDate,
                                            UpdatedBy = x.UpdatedBy,
                                            UpdatedDate = x.UpdatedDate,
                                            UpdatedUTCDate = x.UpdatedUTCDate
                                        }).AsQueryable();
            var categoryData = new PagedList<CategoryResponseDto>(categoryResponseData, dataTableFilterDto);
            return new JsonRepsonse<CategoryResponseDto>(dataTableFilterDto.Draw, categoryData.TotalCount, categoryData.TotalCount, categoryData);
        }

        public async Task<CategoryResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var categoryData = await GetCategoryById(Id, cancellationToken);
            return _mapper.Map<CategoryResponseDto>(categoryData);
        }

        public async Task<CategoryRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var categoryData = await GetCategoryById(Id, cancellationToken);
            return _mapper.Map<CategoryRequestDto>(categoryData);
        }

        public async Task<CategoryRequestDto> CreateCategory(CategoryRequestDto model, CancellationToken cancellationToken)
        {
            var categoryToInsert = _mapper.Map<LookupValues>(model);
            categoryToInsert.IsDeleted = false;
            categoryToInsert.CreatedBy = _currentUser.UserId;
            categoryToInsert.CreatedDate = DateTime.Now;
            categoryToInsert.CreatedUTCDate = DateTime.UtcNow;
            var data = await _unitOfWorkDA.LookupValuesDA.CreateLookupValue(categoryToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<CategoryRequestDto>(data);
            return _mappedUser;
        }

        public async Task<CategoryRequestDto> UpdateCategory(int Id, CategoryRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetCategoryById(Id, cancellationToken);
            UpdateCategory(oldData);
            MapToDbObject(model, oldData);
            var categoryUpdatedata = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<CategoryRequestDto>(categoryUpdatedata);
            return _mappedUser;
        }

        public async Task<CategoryRequestDto> DeleteCategory(int Id, CancellationToken cancellationToken)
        {
            var categoryToUpdate = await GetCategoryById(Id, cancellationToken);
            categoryToUpdate.IsDeleted = true;
            UpdateCategory(categoryToUpdate);
            var categoryDeletedata = await _unitOfWorkDA.LookupValuesDA.UpdateLookupValue(Id, categoryToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<CategoryRequestDto>(categoryDeletedata);
            return _mappedUser;
        }

        #region PrivateMethods

        private void UpdateCategory(LookupValues oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(CategoryRequestDto model, LookupValues oldData)
        {
            oldData.LookupValueName = model.LookupValueName;
            oldData.LookupValueId = model.LookupValueId;
        }

        private async Task<LookupValues> GetCategoryById(int Id, CancellationToken cancellationToken)
        {
            var categoryDataById = await _unitOfWorkDA.LookupValuesDA.GetById(Id, cancellationToken);
            if (categoryDataById == null)
            {
                throw new Exception("Category not found");
            }
            return categoryDataById;
        }

        #endregion PrivateMethods
    }
}