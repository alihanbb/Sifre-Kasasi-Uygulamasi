using AppRepository.Entities.Dtos;

namespace AppServices.Abstract
{
    public interface IUserManagementService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<List<RoleDto>> GetUserRolesAsync(int userId);
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<bool> UpdateUserRolesAsync(UpdateUserRolesDto updateUserRolesDto);
    }
}