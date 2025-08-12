using AppRepository.Entities.Dtos;
using AppServices.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "manager")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;

        public ManagerController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                // Debug için kullanıcı bilgisini loglayalım
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value);
                
                Console.WriteLine($"Manager API: User ID: {userId}, Username: {userName}, Roles: {string.Join(",", roles)}");

                var users = await _userManagementService.GetAllUsersAsync();
                Console.WriteLine($"Manager API: Retrieved {users?.Count ?? 0} users");
                
                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Manager API Error: {ex.Message}");
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            try
            {
                var user = await _userManagementService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Kullanıcı bulunamadı");
                }

                var userRoles = await _userManagementService.GetUserRolesAsync(userId);
                return Ok(userRoles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Manager API Error: {ex.Message}");
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _userManagementService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Manager API Error: {ex.Message}");
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPut("user/{userId}/roles")]
        public async Task<IActionResult> UpdateUserRoles(int userId, [FromBody] List<string> selectedRoles)
        {
            try
            {
                var updateDto = new UpdateUserRolesDto
                {
                    UserId = userId,
                    SelectedRoles = selectedRoles ?? new List<string>()
                };

                var result = await _userManagementService.UpdateUserRolesAsync(updateDto);
                
                if (!result)
                {
                    return BadRequest("Kullanıcı rolleri güncellenirken hata oluştu");
                }

                return Ok("Kullanıcı rolleri başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Manager API Error: {ex.Message}");
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}
