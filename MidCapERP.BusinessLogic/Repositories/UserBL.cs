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

        public async Task<IQueryable<ApplicationUser>> GetAllUsers(CancellationToken cancellationToken)
        {
            var getUser = await GetAllUsersData(cancellationToken);
            return getUser;
        }

        public async Task<IList<ApplicationRole>> GetAllRoles(CancellationToken cancellationToken)
        {
            var getRole = await _unitOfWorkDA.UserDA.GetRoles(cancellationToken);
            var rolesByTenant = getRole.Where(x => x.TenantId == _currentUser.TenantId).ToList();
            return rolesByTenant;
        }

        public async Task<JsonRepsonse<UserResponseDto>> GetFilterUserData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken)
        {
            var userAllData = await GetAllUsersData(cancellationToken);
            var users = from x in userAllData
                        join y in await _unitOfWorkDA.UserTenantMappingDA.GetAll(cancellationToken)
                                   on new { x.UserId } equals new { y.UserId }
                        where y.TenantId == _currentUser.TenantId
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
            // Get ApplicationUser by Id
            var userAllData = await GetAllUsersData(cancellationToken);
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

            // Get User Role by roleId
            var roleId = _unitOfWorkDA.UserDA.GetUserRoleId(applicationUser.Id, cancellationToken);
            var roleDataById = await _roleManager.FindByIdAsync(roleId.Result);
            if (roleDataById != null)
                applicationUser.AspNetRole = roleDataById.NormalizedName;
            return applicationUser;
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
            applicationUser.EmailConfirmed = true;
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
            var userAllData = await GetAllUsersData(cancellationToken);
            var oldApplicationUserData = userAllData.Where(p => p.UserId == Id).FirstOrDefault();
            oldApplicationUserData.FirstName = model.FirstName;
            oldApplicationUserData.LastName = model.LastName;
            oldApplicationUserData.PhoneNumber = model.PhoneNumber;
            var updateUser = await _unitOfWorkDA.UserDA.UpdateUser(_mapper.Map<ApplicationUser>(oldApplicationUserData));

            // Get selected role details from AspNetUserRoles and AspNetRoles
            var rolesData = _unitOfWorkDA.UserDA.GetUserRoleData(Convert.ToString(oldApplicationUserData.Id), cancellationToken).Result.FirstOrDefault();
            var oldRoleNameData = await _roleManager.FindByIdAsync(rolesData.RoleId);

            //Remove old UserRole
            await _userManager.RemoveFromRoleAsync(oldApplicationUserData, oldRoleNameData.Name);

            //Add Updated UserRole
            await _userManager.AddToRoleAsync(oldApplicationUserData, model.AspNetRole);

            return _mapper.Map<UserRequestDto>(oldApplicationUserData);
        }

        public async Task<UserRequestDto> DeleteUser(int Id, CancellationToken cancellationToken)
        {
            // InActive AspNetUser
            var userAllData = await GetAllUsersData(cancellationToken);
            var userById = userAllData.Where(x => x.UserId == Id).FirstOrDefault();
            userById.IsActive = false;
            userById.EmailConfirmed = false;
            userById.IsDeleted = true;
            await _unitOfWorkDA.UserDA.UpdateUser(_mapper.Map<ApplicationUser>(userById));
            return _mapper.Map<UserRequestDto>(userById);
        }

        #region Private Method

        private async Task<IQueryable<ApplicationUser>> GetAllUsersData(CancellationToken cancellationToken)
        {
            var getAllUser = await _unitOfWorkDA.UserDA.GetUsers(cancellationToken);
            if (getAllUser == null)
            {
                throw new Exception("Users data not found");
            }
            return getAllUser;
        }

        #endregion Private Method
    }
}