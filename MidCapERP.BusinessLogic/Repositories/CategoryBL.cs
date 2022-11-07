﻿using AutoMapper;
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
            var data = await _unitOfWorkDA.CategoriesDA.GetAll(cancellationToken);
            return _mapper.Map<List<CategoryResponseDto>>(data.Where(x => x.CategoryTypeId == (int)ProductCategoryTypesEnum.Product).ToList());
        }

        public async Task<CategoryResponseDto> GetCategorySearchByCategoryName(string searchName, CancellationToken cancellationToken)
        {
            int lookupId = await GetLookupId(cancellationToken);
            var categoryAllData = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            return _mapper.Map<CategoryResponseDto>(categoryAllData.FirstOrDefault(x => x.LookupId == lookupId && x.LookupValueName == searchName));
        }

        public async Task<JsonRepsonse<CategoryResponseDto>> GetFilterCategoryData(CategoryDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var categoryAllData = await _unitOfWorkDA.CategoriesDA.GetAll(cancellationToken);
            var categoryResponseData = (from x in categoryAllData
                                        select new CategoryResponseDto()
                                        {
                                            CategoryId = x.CategoryId,
                                            CategoryName = x.CategoryName,
                                            IsDeleted = x.IsDeleted,
                                            CreatedBy = (int)x.CreatedBy,
                                            CreatedDate = x.CreatedDate,
                                            CreatedUTCDate = x.CreatedUTCDate,
                                            UpdatedBy = (int?)x.UpdatedBy,
                                            UpdatedDate = x.UpdatedDate,
                                            UpdatedUTCDate = x.UpdatedUTCDate,
                                        }).AsQueryable();
            var categoryFilteredData = FilterCategoryData(dataTableFilterDto, categoryResponseData);
            var categoryData = new PagedList<CategoryResponseDto>(categoryFilteredData, dataTableFilterDto);
            return new JsonRepsonse<CategoryResponseDto>(dataTableFilterDto.Draw, categoryData.TotalCount, categoryData.TotalCount, categoryData);
        }

        public async Task<CategoryResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var categoryData = await GetCategoryById(Id, cancellationToken);
            return _mapper.Map<CategoryResponseDto>(categoryData);
        }

        public async Task<CategoryRequestDto> GetById(long Id, CancellationToken cancellationToken)
        {
            var categoryData = await GetCategoryById(Id, cancellationToken);
            return _mapper.Map<CategoryRequestDto>(categoryData);
        }

        public async Task<CategoryRequestDto> CreateCategory(CategoryRequestDto model, CancellationToken cancellationToken)
        {
            var categoryToInsert = _mapper.Map<Categories>(model);
            categoryToInsert.IsDeleted = false;
            categoryToInsert.CreatedBy = _currentUser.UserId;
            categoryToInsert.CreatedDate = DateTime.Now;
            categoryToInsert.CreatedUTCDate = DateTime.UtcNow;
            categoryToInsert.TenantId = _currentUser.TenantId;
            var data = await _unitOfWorkDA.CategoriesDA.CreateCategory(categoryToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<CategoryRequestDto>(data);
            return _mappedUser;
        }

        public async Task<CategoryRequestDto> UpdateCategory(long Id, CategoryRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await GetCategoryById(Id, cancellationToken);
            UpdateCategory(oldData);
            MapToDbObject(model, oldData);
            var categoryUpdatedata = await _unitOfWorkDA.CategoriesDA.UpdateCategory(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<CategoryRequestDto>(categoryUpdatedata);
            return _mappedUser;
        }

        public async Task<CategoryRequestDto> DeleteCategory(int Id, CancellationToken cancellationToken)
        {
            var categoryToUpdate = await GetCategoryById(Id, cancellationToken);
            categoryToUpdate.IsDeleted = true;
            UpdateCategory(categoryToUpdate);
            var categoryDeletedata = await _unitOfWorkDA.CategoriesDA.UpdateCategory(Id, categoryToUpdate, cancellationToken);
            var _mappedUser = _mapper.Map<CategoryRequestDto>(categoryDeletedata);
            return _mappedUser;
        }

        #region PrivateMethods

        private void UpdateCategory(Categories oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(CategoryRequestDto model, Categories oldData)
        {
            oldData.CategoryName = model.CategoryName;
            oldData.IsFixedPrice = model.IsFixedPrice;
            oldData.CategoryTypeId = model.CategoryTypeId;
            oldData.RSPPercentage = Convert.ToDecimal(model.RSPPercentage);
            oldData.WSPPercentage = Convert.ToDecimal(model.WSPPercentage);
        }

        private async Task<Categories> GetCategoryById(long Id, CancellationToken cancellationToken)
        {
            var categoryDataById = await _unitOfWorkDA.CategoriesDA.GetById(Id, cancellationToken);
            if (categoryDataById == null)
            {
                throw new Exception("Category not found");
            }
            return categoryDataById;
        }

        private async Task<int> GetLookupId(CancellationToken cancellationToken)
        {
            var lookupsAllData = await _unitOfWorkDA.LookupsDA.GetAll(cancellationToken);
            var lookupId = lookupsAllData.Where(x => x.LookupName == nameof(MasterPagesEnum.Category)).Select(x => x.LookupId).FirstOrDefault();
            return lookupId;
        }

        private static IQueryable<CategoryResponseDto> FilterCategoryData(CategoryDataTableFilterDto categoryDataTableFilterDto, IQueryable<CategoryResponseDto> categoryResponseDto)
        {
            if (categoryDataTableFilterDto != null)
            {
                if (!string.IsNullOrEmpty(categoryDataTableFilterDto.CategoryName))
                {
                    categoryResponseDto = categoryResponseDto.Where(p => p.CategoryName.StartsWith(categoryDataTableFilterDto.CategoryName));
                }
            }

            return categoryResponseDto;
        }

        #endregion PrivateMethods
    }
}