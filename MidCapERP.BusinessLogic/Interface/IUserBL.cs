using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.User;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IUserBL
    {
        public Task<IQueryable<ApplicationUser>> GetAllUsers(CancellationToken cancellationToken);

        public Task<IList<ApplicationRole>> GetAllRoles(CancellationToken cancellationToken);

        public Task<JsonRepsonse<UserResponseDto>> GetFilterUserData(UserDataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        public Task<UserRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<UserRequestDto> CreateUser(UserRequestDto model, CancellationToken cancellationToken);

        public Task<UserRequestDto> UpdateUser(int Id, UserRequestDto model, CancellationToken cancellationToken);

        public Task<UserRequestDto> DeleteUser(int Id, CancellationToken cancellationToken);
    }
}