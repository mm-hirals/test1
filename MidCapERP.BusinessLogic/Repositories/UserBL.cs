using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MidCapERP.BusinessLogic.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.Paging;
using MidCapERP.Dto.User;
using MidCapERP.Dto.UserTenantMapping;

namespace MidCapERP.BusinessLogic.Repositories
{
    public class UserBL : IUserBL
    {
        private readonly IUnitOfWorkDA _unitOfWorkDA;
        private readonly CurrentUser _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUserTenantMappingBL _userTenantMappingBL;

        public UserBL(IUnitOfWorkDA unitOfWorkDA, CurrentUser currentUser, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMapper mapper, IUserTenantMappingBL userTenantMappingBL)
        {
            _unitOfWorkDA = unitOfWorkDA;
            _currentUser = currentUser;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userTenantMappingBL = userTenantMappingBL;
        }

        public async Task<IQueryable<ApplicationUser>> GetAll(CancellationToken cancellationToken)
        {
            var getUser = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            return getUser;
        }

        public async Task<JsonRepsonse<UserResponseDto>> GetFilterUserData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var userAllData = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var users = from x in userAllData
                        orderby x.UserId ascending
                        select new UserResponseDto
                        {
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            UserName = x.UserName,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            UserId = x.UserId
                        };
            var userData = new PagedList<UserResponseDto>(users, dataTableFilterDto);
            return new JsonRepsonse<UserResponseDto>(dataTableFilterDto.Draw, userData.TotalCount, userData.TotalCount, userData);
        }

        public async Task<UserRequestDto> GetById(int Id, CancellationToken cancellationToken)
        {
            var applicationUserById = await GetApplicationUserById(Id, cancellationToken);
            return applicationUserById;
        }

        public async Task<UserRequestDto> CreateUser(UserRequestDto model, CancellationToken cancellationToken)
        {
            // Add User into AspNetUser
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.FirstName = model.FirstName;
            applicationUser.LastName = model.LastName;
            applicationUser.UserName = model.UserName;
            applicationUser.Email = model.Email;
            applicationUser.PhoneNumber = model.PhoneNumber;
            applicationUser.IsActive = true;
            await _unitOfWorkDA.UserDA.CreateUser(applicationUser, model.Password);

            // Add UserId and TenantId into UserTenantMapping
            UserTenantMappingRequestDto userTenantData = new UserTenantMappingRequestDto();
            userTenantData.TenantId = _currentUser.TenantId;
            userTenantData.UserId = applicationUser.UserId;
            await _userTenantMappingBL.CreateUserTenant(userTenantData, cancellationToken);

            // Add into AspNetRole
            await _userManager.AddToRoleAsync(applicationUser, model.AspNetRole);

            return _mapper.Map<UserRequestDto>(applicationUser);
        }

        public async Task<UserRequestDto> UpdateUser(int Id, UserRequestDto model, CancellationToken cancellationToken)
        {
            // Update AspNetUser
            var userAllData = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var oldApplicationUserData = userAllData.Where(p => p.UserId == Id).FirstOrDefault();
            oldApplicationUserData.FirstName = model.FirstName;
            oldApplicationUserData.LastName = model.LastName;
            oldApplicationUserData.PhoneNumber = model.PhoneNumber;
            var updateUser = await _unitOfWorkDA.UserDA.UpdateUser(_mapper.Map<ApplicationUser>(oldApplicationUserData));

            // Get selected role details
            var roleNameData = await _roleManager.FindByNameAsync(model.AspNetRole);
            string oldId = Convert.ToString(oldApplicationUserData.Id);

            var rolesData = _unitOfWorkDA.UserDA.GetByIdentityUserRoleData(oldId, cancellationToken).Result.FirstOrDefault();

            //Remove old UserRole
            var updatedUser = userAllData.Where(p => p.UserId == Id).FirstOrDefault();
            var data = await _userManager.RemoveFromRoleAsync(updatedUser, rolesData.RoleId);

            //Added Updated UserRole
            await _userManager.AddToRoleAsync(updatedUser, model.AspNetRole);

            return _mapper.Map<UserRequestDto>(oldApplicationUserData);

        }

        public async Task<UserRequestDto> DeleteUser(int Id, CancellationToken cancellationToken)
        {
            // InActive AspNetUser
            var userAllData = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var userById = userAllData.Where(x => x.UserId == Id).FirstOrDefault();
            userById.IsActive = false;
            var updateUser = await _unitOfWorkDA.UserDA.UpdateUser(_mapper.Map<ApplicationUser>(userById));
            return _mapper.Map<UserRequestDto>(userById);
        }

        #region Private Method

        private async Task<UserRequestDto> GetApplicationUserById(int Id, CancellationToken cancellationToken)
        {
            // Get ApplicationUser by Id
            var userAllData = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            var applicationUser = (from x in userAllData
                                   join y in await _unitOfWorkDA.UserTenantMappingDA.GetAll(cancellationToken)
                                   on new { x.UserId } equals new { y.UserId }
                                   where x.UserId == Id
                                   select new UserRequestDto
                                   {
                                       Id = x.Id,
                                       FirstName = x.FirstName,
                                       LastName = x.LastName,
                                       UserName = x.UserName,
                                       Email = x.Email,
                                       PhoneNumber = x.PhoneNumber,
                                       UserId = x.UserId,
                                       TenantId = y.TenantId,
                                       UserTenantMappingId = y.UserTenantMappingId
                                   }).FirstOrDefault();

            // Get User Role
            var roleId = _unitOfWorkDA.UserDA.GetByIdentityUserRoleId(applicationUser.Id, cancellationToken);
            var roleById = _unitOfWorkDA.AspNetRoleDA.GetAll(cancellationToken).Result.Where(x => x.Id == roleId.Result).FirstOrDefault();
            if(roleById != null)
                applicationUser.AspNetRole = roleById.NormalizedName;
            return applicationUser;
        }

        #endregion Private Method
    }
}