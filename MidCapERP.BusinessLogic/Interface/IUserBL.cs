using MidCapERP.DataEntities.Models;
using MidCapERP.Dto.DataGrid;
using MidCapERP.Dto.User;

namespace MidCapERP.BusinessLogic.Interface
{
    public interface IUserBL
    {
        public Task<IQueryable<ApplicationUser>> GetAll(CancellationToken cancellationToken);

        public Task<JsonRepsonse<UserResponseDto>> GetFilterUserData(DataTableFilterDto dataTableFilterDto, CancellationToken cancellationToken);

        //public Task<UserResponseDto> GetDetailsById(int Id, CancellationToken cancellationToken);

        public Task<UserRequestDto> GetById(int Id, CancellationToken cancellationToken);

        public Task<UserRequestDto> CreateUser(UserRequestDto model, CancellationToken cancellationToken);

        public Task<UserRequestDto> UpdateUser(int Id, UserRequestDto model, CancellationToken cancellationToken);

        public Task<UserRequestDto> DeleteUser(int Id, CancellationToken cancellationToken);
    }
}