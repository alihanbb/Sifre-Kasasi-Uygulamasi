using System.Net.Http.Headers;
using AppRepository.Entities;
using AppRepository.Entities.Dtos;
using AppServices.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    [Authorize(Roles = "manager,member")]
    public class AdminMemberController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly UserManager<AppUser> userManager;
        private readonly IJwtService jwtService;

        public AdminMemberController(IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager, IJwtService jwtService)
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
        public IActionResult Members()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllMembersJson()
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });

                var token = await jwtService.GenerateTokenAsync(user);
                
                // Debug için token ve kullanıcı bilgisini loglayalım
                Console.WriteLine($"Web: Generated token for user {user.UserName} (ID: {user.Id})");
                Console.WriteLine($"Web: Token length: {token.Length}");
                
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var apiUrl = $"https://localhost:7204/api/member/user/{user.Id}";
                Console.WriteLine($"Web: Calling API: {apiUrl}");
                
                var response = await client.GetAsync(apiUrl);

                Console.WriteLine($"Web: API Response Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Web: API Error Content: {errorContent}");
                    return Json(new { success = false, message = $"API isteği başarısız oldu: {response.StatusCode}. Detay: {errorContent}" });
                }

                var members = await response.Content.ReadFromJsonAsync<List<MemberDto>>();
                Console.WriteLine($"Web: Retrieved {members?.Count ?? 0} members");
                
                return Json(new { success = true, message = "Üyeler başarıyla getirildi.", data = members });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Web: Exception in GetAllMembersJson: {ex.Message}");
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<JsonResult> UpdateMemberJson(int id)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                   return Json(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });
                  
                var token = await jwtService.GenerateTokenAsync(user);
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var response = await client.GetAsync($"https://localhost:7204/api/member/{id}/user/{user.Id}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"API isteği başarısız oldu: {response.StatusCode}. Detay: {errorContent}" });
                }
                
                var member = await response.Content.ReadFromJsonAsync<MemberDto>();

                if (member == null)
                        return Json(new { success = false, message = "API'den boş yanıt alındı veya deserializasyon hatası oluştu." });
                return Json(new { success = true, data = member });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateMemberJson([FromBody] CreateMemberDto createUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, errors = ModelState });
                }

                var user = await userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });

                var token = await jwtService.GenerateTokenAsync(user);
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var response = await client.PostAsJsonAsync($"https://localhost:7204/api/member/user/{user.Id}", createUser);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"API isteği başarısız oldu: {response.StatusCode}. Detay: {errorContent}" });
                }
                    
                return Json(new { success = true, message = "Kullanıcı bilgileri başarıyla eklendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateMemberJson([FromBody] UpdateMemberDto updateUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, errors = ModelState });
         
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });
                    
                var token = await jwtService.GenerateTokenAsync(user);
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var response = await client.PutAsJsonAsync($"https://localhost:7204/api/member/{updateUser.Id}/user/{user.Id}", updateUser);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"API isteği başarısız oldu: {response.StatusCode}. Detay: {errorContent}" });
                }
                      
                return Json(new { success = true, message = "Kullanıcı bilgileri başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteMemberJson(int id)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bilgisi bulunamadı." });
             
                var token = await jwtService.GenerateTokenAsync(user);
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                var response = await client.DeleteAsync($"https://localhost:7204/api/member/{id}/user/{user.Id}");
        
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"API isteği başarısız oldu: {response.StatusCode}. Detay: {errorContent}" });
                }
                    
                return Json(new { success = true, message = "Kullanıcı bilgileri başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Hata oluştu: {ex.Message}" });
            }
        }
    }
}