using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.Admin.Middleware
{
    public class CurrentUserActionFilter : IAsyncActionFilter
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly CurrentUser _currentUser;

        public CurrentUserActionFilter(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, CurrentUser currentUser)
        {
            _currentUser = currentUser;
            _userManager = userManager;
            _roleManager = roleManager;
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
                        _currentUser.UserId = user.UserID;
                        _currentUser.Name = user.FirstName;
                        _currentUser.FullName = user.FullName;
                        _currentUser.EmailAddress = user.Email;
                        _currentUser.RoleId = userRoles?.Id;
                        _currentUser.Role = userRoles?.Name;
                        //_currentUser.TenantId = ;
                    }
                }
            }
        }
    }
}
