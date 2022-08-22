using Microsoft.AspNetCore.Identity;
using MidCapERP.DataEntities.Models;

namespace MidCapERP.DataAccess.Interface
{
    public interface IUserDA
    {
        public Task<IQueryable<ApplicationUser>> GetUsers(CancellationToken cancellationToken);

        public Task<string> GetByIdentityUserRoleId(string Id, CancellationToken cancellationToken);

        public Task<IQueryable<IdentityUserRole<string>>> GetByIdentityUserRoleData(string Id, CancellationToken cancellationToken);

        public Task<IdentityResult> CreateUser(ApplicationUser model, string password);

        public Task<IdentityResult> UpdateUser(ApplicationUser model);
    }
}