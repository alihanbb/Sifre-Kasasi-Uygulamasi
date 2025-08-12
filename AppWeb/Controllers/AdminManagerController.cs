using System.Net.Http.Headers;
using AppRepository.Entities;
using AppRepository.Entities.Dtos;
using AppServices.Abstract;
using AppWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    [Authorize(Roles = "manager")]
    public class AdminManagerController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly UserManager<AppUser> userManager;
        private readonly IJwtService jwtService;

        public AdminManagerController(IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager, IJwtService jwtService)
        {
            this.httpClientFactory = httpClientFactory;
            this.userManager = userManager;
            this.jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetUsers()
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });

                var token = await jwtService.GenerateTokenAsync(user); 
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"https://localhost:7204/api/manager/users");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"API isteği başarısız oldu: {response.StatusCode}. Detay: {errorContent}" });
                }

                var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
                var userViewModels = users?.Select(u => new RoleViewModel
                {
                    User = new AppUser 
                    { 
                        Id = u.Id, 
                        UserName = u.UserName, 
                        Email = u.Email, 
                        FirstName = u.FirstName, 
                        LastName = u.LastName 
                    },
                    Roles = u.Roles
                }).ToList() ?? new List<RoleViewModel>();
                
                return Json(new { success = true, data = userViewModels });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<JsonResult> UpdateRole(int userId)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });

                var token = await jwtService.GenerateTokenAsync(user);
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"https://localhost:7204/api/manager/user/{userId}/roles");
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"API isteği başarısız oldu: {response.StatusCode}. Detay: {errorContent}" });
                }
                var roles = await response.Content.ReadFromJsonAsync<List<RoleDto>>();
                var roleViewModels = roles?.Select(r => new RoleViewModel
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    IsSelected = r.IsSelected,
                    User = new AppUser { Id = userId }
                }).ToList() ?? new List<RoleViewModel>();
                return Json(roleViewModels);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRole(int userId, List<RoleViewModel> roles)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });

                var token = await jwtService.GenerateTokenAsync(user);
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var selectedRoles = roles?.Where(r => r.IsSelected && !string.IsNullOrEmpty(r.RoleName))
                                         .Select(r => r.RoleName).ToList() ?? new List<string>();             
                var response = await client.PutAsJsonAsync($"https://localhost:7204/api/manager/user/{userId}/roles", selectedRoles);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = errorContent});
                }
                return Json(new { success = true, message = "Kullanıcı rolleri başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Manager Web: Exception in UpdateRole (POST): {ex.Message}");
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        } 
    }
}
