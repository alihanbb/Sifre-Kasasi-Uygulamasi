using AppRepository.Entities;
using AppRepository.Entities.Dtos;
using AppServices.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppServices.Concrete
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserManagementService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? "",
                    Email = user.Email ?? "",
                    FirstName = user.FirstName ?? "",
                    LastName = user.LastName ?? "",
                    Roles = roles.ToList()
                });
            }

            return userDtos;
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Roles = roles.ToList()
            };
        }

        public async Task<List<RoleDto>> GetUserRolesAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new List<RoleDto>();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            return allRoles.Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? "",
                IsSelected = userRoles.Contains(role.Name ?? "")
            }).ToList();
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(role => new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? "",
                IsSelected = false
            }).ToList();
        }

        public async Task<bool> UpdateUserRolesAsync(UpdateUserRolesDto updateUserRolesDto)
        {
            var user = await _userManager.FindByIdAsync(updateUserRolesDto.UserId.ToString());
            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return false;
            }
            if (updateUserRolesDto.SelectedRoles.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, updateUserRolesDto.SelectedRoles);
                if (!addResult.Succeeded)
                    return false;
            }
            else
            {
                var defaultResult = await _userManager.AddToRoleAsync(user, "member");
                if (!defaultResult.Succeeded)
                    return false;
            }

            return true;
        }
    }
}