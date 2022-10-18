using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using MidCapERP.BusinessLogic.UnitOfWork;
using MidCapERP.Core.Constants;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.Admin.Middleware
{
    public class CurrentUserActionFilter : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly CurrentUser _currentUser;
        private readonly IUnitOfWorkBL _unitOfWorkBL;

        public CurrentUserActionFilter(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, CurrentUser currentUser, IUnitOfWorkBL unitOfWorkBL)
        {
            _currentUser = currentUser;
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWorkBL = unitOfWorkBL;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await CurrentUserInfo(context);
            await next();
        }

        private async Task CurrentUserInfo(ActionExecutingContext context)
        {
            if (context.HttpContext.User != null)
            {
                if (context.HttpContext.User.Identity != null)
                {
                    var user = await _userManager.GetUserAsync(context.HttpContext.User);
                    if (user != null)
                    {
                        var userRoleNames = await _userManager.GetRolesAsync(user);
                        var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name)).FirstOrDefault();
                        _currentUser.UserId = user.UserId;
                        _currentUser.Name = user.FirstName;
                        _currentUser.FullName = user.FullName;
                        _currentUser.EmailAddress = user.Email;
                        _currentUser.RoleId = userRoles?.Id;
                        _currentUser.Role = userRoles?.Name;
                        if (context.HttpContext.Request.Cookies[ApplicationIdentityConstants.TenantCookieName] != null)
                        {
                            _currentUser.TenantId = Convert.ToInt32(MagnusMinds.Utility.Encryption.Decrypt(context.HttpContext.Request.Cookies[ApplicationIdentityConstants.TenantCookieName], true, ApplicationIdentityConstants.EncryptionSecret));
                            var userTenantData = await _unitOfWorkBL.UserTenantMappingBL.GetAll(new CancellationToken());
                            if (userTenantData != null)
                            {
                                _currentUser.TenantName = userTenantData?.FirstOrDefault(x => x.TenantId == _currentUser.TenantId)?.TenantName;
                                _currentUser.IsMultipleTenant = userTenantData.Count() == 1 ? false : true;
                            }
                        }
                        _currentUser.Role = userRoles?.Name.Replace("_" + Convert.ToString(_currentUser.TenantId), "");
                    }
                }
            }
        }
    }
}