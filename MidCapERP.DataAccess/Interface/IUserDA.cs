using Microsoft.AspNetCore.Identity;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IUserDA
    {
        public Task<IQueryable<ApplicationUser>> GetUsers(CancellationToken cancellationToken);

        public Task<IQueryable<ApplicationRole>> GetRoles(CancellationToken cancellationToken);

        public Task<string> GetUserRoleId(string Id, CancellationToken cancellationToken);

        public Task<IQueryable<IdentityUserRole<string>>> GetUserRoleData(string Id, CancellationToken cancellationToken);

        public Task<IdentityResult> CreateUser(ApplicationUser model, string password);

        public Task<IdentityResult> UpdateUser(ApplicationUser model);
    }
}