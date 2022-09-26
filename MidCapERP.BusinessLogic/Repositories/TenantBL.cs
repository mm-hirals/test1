using AutoMapper;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.Tenant;

namespace MidCapERP.BusinessLogic.Repositories
{
	public class TenantBL : ITenantBL
	{
		private IUnitOfWorkDA _unitOfWorkDA;
		public readonly IMapper _mapper;
		private readonly CurrentUser _currentUser;

		public TenantBL(IUnitOfWorkDA unitOfWorkDA, IMapper mapper, CurrentUser currentUser)
		{
			_unitOfWorkDA = unitOfWorkDA;
			_mapper = mapper;
			_currentUser = currentUser;
		}

		public async Task<IEnumerable<TenantResponseDto>> GetAll(CancellationToken cancellationToken)
		{
			var data = await _unitOfWorkDA.CustomersDA.GetAll(cancellationToken);
			return _mapper.Map<List<TenantResponseDto>>(data.ToList());
		}

		public async Task<JsonRepsonse<TenantResponseDto>> GetFilterCustomersData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
		{
			var TenantAllData = await _unitOfWorkDA.TenantDA.GetAll(cancellationToken);
			var TenantData = new PagedList<TenantResponseDto>(_mapper.Map<List<TenantResponseDto>>(TenantAllData).AsQueryable(), dataTableFilterDto);
			return new JsonRepsonse<TenantResponseDto>(dataTableFilterDto.Draw, TenantData.TotalCount, TenantData.TotalCount, TenantData);
		}

		public async Task<TenantResponseDto> GetById(int Id, CancellationToken cancellationToken)
		{
			var data = await TenantGetById(Id, cancellationToken);
			return _mapper.Map<TenantResponseDto>(data);
		}

		public async Task<TenantRequestDto> UpdateTenant(int Id, TenantRequestDto model, CancellationToken cancellationToken)
		{
			var oldData = await TenantGetById(Id, cancellationToken);
			oldData.UpdatedBy = _currentUser.UserId;
			oldData.UpdatedDate = DateTime.Now;
			oldData.UpdatedUTCDate = DateTime.UtcNow;
			MapToDbObject(model, ref oldData);
			var data = await _unitOfWorkDA.TenantDA.UpdateTenant(Id, oldData, cancellationToken);
			return _mapper.Map<TenantRequestDto>(data);
		}

		#region PrivateMethods

		private static void MapToDbObject(TenantRequestDto model, ref Tenant oldData)
		{
			oldData.TenantName = model.TenantName;
			oldData.FirstName = model.FirstName;
			oldData.EmailId = model.EmailId;
			oldData.PhoneNumber = model.PhoneNumber;
			oldData.LogoPath = model.LogoPath;
			oldData.StreetAddress1 = model.StreetAddress1;
			oldData.StreetAddress2 = model.StreetAddress2;
			oldData.Landmark = model.Landmark;
			oldData.City = model.City;
			oldData.State = model.State;
			oldData.Pincode = model.Pincode;
			oldData.WebsiteURL = model.WebsiteURL;
			oldData.TwitterURL = model.TwitterURL;
			oldData.FacebookURL = model.FacebookURL;
			oldData.InstagramURL = model.InstagramURL;
			oldData.GSTNo = model.GSTNo;
			oldData.RetailerPercentage = model.RetailerPercentage;
			oldData.WholeSellerPercentage = model.WholeSellerPercentage;
			oldData.ArchitectDiscount = model.ArchitectDiscount;
		}

		private async Task<Tenant> TenantGetById(int Id, CancellationToken cancellationToken)
		{
			var data = await _unitOfWorkDA.TenantDA.GetById(Id, cancellationToken);
			if (data == null)
			{
				throw new Exception("Tenant not found");
			}
			return data;
		}

		#endregion PrivateMethods
	}
}