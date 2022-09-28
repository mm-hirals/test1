using AutoMapper;
using MidCapERP.BusinessLogic.Constants;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.BusinessLogic.Services.FileStorage;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.Constants;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Frames;
using MidCapERP.Dto.Paging;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class FrameBL : IFrameBL
    {
        private IUnitOfWorkDA _unitOfWorkDA;
        public readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly IFileStorageService _fileStorageService;

        public FrameBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser, IFileStorageService fileStorageService)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _mapper = mapper;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<FrameResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var frameData = await _unitOfWorkDA.FrameDA.GetAll(cancellationToken);
            return _mapper.Map<List<FrameResponseDto>>(frameData.ToList());
        }

        public async Task<JsonRepsonse<FrameResponseDto>> GetFilterFrameData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var frameAllData = await _unitOfWorkDA.FrameDA.GetAll(cancellationToken);
            var lookupValueAll = await _unitOfWorkDA.LookupValuesDA.GetAll(cancellationToken);
            var frameTypeData = lookupValueAll.Where(x => x.LookupId == (int)MasterPagesEnum.FrameType);
            var companyData = lookupValueAll.Where(x => x.LookupId == (int)MasterPagesEnum.Company);
            var unitData = lookupValueAll.Where(x => x.LookupId == (int)MasterPagesEnum.Unit);
            var frameResponseData = (from x in frameAllData
                                     join y in frameTypeData on new { FrameTypeId = x.FrameTypeId } equals new { FrameTypeId = y.LookupValueId }
                                     join z in companyData on new { CompanyId = x.CompanyId } equals new { CompanyId = z.LookupValueId }
                                     join a in unitData on new { UnitId = x.UnitId } equals new { UnitId = a.LookupValueId }
                                     select new FrameResponseDto()
                                     {
                                         FrameId = x.FrameId,
                                         FrameTypeName = y.LookupValueName,
                                         Title = x.Title,
                                         ModelNo = x.ModelNo,
                                         CompanyName = z.LookupValueName,
                                         UnitName = a.LookupValueName,
                                         UnitPrice = x.UnitPrice,
                                         IsDeleted = x.IsDeleted,
                                         CreatedBy = x.CreatedBy,
                                         CreatedDate = x.CreatedDate,
                                         CreatedUTCDate = x.CreatedUTCDate,
                                         UpdatedBy = x.UpdatedBy,
                                         UpdatedDate = x.UpdatedDate,
                                         UpdatedUTCDate = x.UpdatedUTCDate
                                     }).AsQueryable();
            var frameData = new PagedList<FrameResponseDto>(frameResponseData, dataTableFilterDto);
            return new JsonRepsonse<FrameResponseDto>(dataTableFilterDto.Draw, frameData.TotalCount, frameData.TotalCount, frameData);
        }

        public async Task<FrameResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken)
        {
            var frameData = await FrameGetById(Id, cancellationToken);
            return _mapper.Map<FrameResponseDto>(frameData);
        }

        public async Task<FrameRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var frameData = await FrameGetById(Id, cancellationToken);
            return _mapper.Map<FrameRequestDto>(frameData);
        }

        public async Task<FrameRequestDto> CreateFrame(FrameRequestDto model, CancellationToken cancellationToken)
        {
            var frameToInsert = _mapper.Map<Frames>(model);
            if (model.UploadImage != null)
                frameToInsert.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Frames);
            frameToInsert.IsDeleted = false;
            frameToInsert.TenantId = _currentUser.TenantId;
            frameToInsert.CreatedBy = _currentUser.UserId;
            frameToInsert.CreatedDate = DateTime.Now;
            frameToInsert.CreatedUTCDate = DateTime.UtcNow;
            var frameData = await _unitOfWorkDA.FrameDA.CreateFrame(frameToInsert, cancellationToken);
            var _mappedUser = _mapper.Map<FrameRequestDto>(frameData);
            return _mappedUser;
        }

        public async Task<FrameRequestDto> UpdateFrame(int Id, FrameRequestDto model, CancellationToken cancellationToken)
        {
            var oldData = await FrameGetById(Id, cancellationToken);
            if (model.UploadImage != null)
                model.ImagePath = await _fileStorageService.StoreFile(model.UploadImage, ApplicationFileStorageConstants.FilePaths.Frames);
            UpdateFrame(oldData);
            MapToDbObject(model, oldData);
            var frameData = await _unitOfWorkDA.FrameDA.UpdateFrame(Id, oldData, cancellationToken);
            var _mappedUser = _mapper.Map<FrameRequestDto>(frameData);
            return _mappedUser;
        }

        public async Task<FrameRequestDto> DeleteFrame(int Id, CancellationToken cancellationToken)
        {
            var frameToUpdate = await FrameGetById(Id, cancellationToken);
            frameToUpdate.IsDeleted = true;
            UpdateFrame(frameToUpdate);
            var frameData = await _unitOfWorkDA.FrameDA.UpdateFrame(Id, frameToUpdate, cancellationToken);
            var _mapped = _mapper.Map<FrameRequestDto>(frameData);
            return _mapped;
        }

        #region Private Method

        private void UpdateFrame(Frames oldData)
        {
            oldData.UpdatedBy = _currentUser.UserId;
            oldData.UpdatedDate = DateTime.Now;
            oldData.UpdatedUTCDate = DateTime.UtcNow;
        }

        private static void MapToDbObject(FrameRequestDto model, Frames oldData)
        {
            oldData.FrameTypeId = model.FrameTypeId;
            oldData.Title = model.Title;
            oldData.ModelNo = model.ModelNo;
            oldData.CompanyId = model.CompanyId;
            oldData.UnitId = model.UnitId;
            oldData.UnitPrice = model.UnitPrice;
            oldData.ImagePath = model.ImagePath;
        }

        private async Task<Frames> FrameGetById(int Id, CancellationToken cancellationToken)
        {
            var frameDataById = await _unitOfWorkDA.FrameDA.GetById(Id, cancellationToken);
            if (frameDataById == null)
            {
                throw new Exception("Frame not found");
            }
            return frameDataById;
        }

        #endregion Private Method
    }
}