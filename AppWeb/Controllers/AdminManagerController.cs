using AppRepository.Entities;
using AppWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWeb.Controllers
{
    [Authorize(Roles ="manager")]
    public class AdminManagerController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        public AdminManagerController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // Normal View döndüren Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // AJAX için kullanıcıları getiren metod
        [HttpGet]
        public async Task<JsonResult> GetUsers()
        {
            try
            {
                var users = await userManager.Users.ToListAsync();
                var userViewModels = new List<RoleViewModel>();

                foreach (var appUser in users)
                {
                    var roles = await userManager.GetRolesAsync(appUser);
                    userViewModels.Add(new RoleViewModel
                    {
                        User = appUser,
                        Roles = roles.ToList()
                    });
                }

                return Json(new { success = true, data = userViewModels });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> UpdateRole(int userId)
        {
            try
            {
                var user = await userManager.FindByIdAsync(userId.ToString());
                
                if (user == null)
                {
                    return Json(new { success = false, message = $"Kullanıcı bulunamadı: {userId}" });
                }
                
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = await roleManager.Roles.ToListAsync();
                
                var roleViews = new List<RoleViewModel>();
                foreach (var item in allRoles)
                {
                    var model = new RoleViewModel
                    {
                        RoleId = item.Id,
                        RoleName = item.Name,
                        IsSelected = userRoles.Contains(item.Name),
                        User = user
                    };
                    roleViews.Add(model);
                }
                return Json(roleViews);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateRole(int userId, List<RoleViewModel> roles)
        {
            try
            {
                if (userId == 0)
                {
                    return Json(new { success = false, message = "Geçersiz kullanıcı ID'si." });
                }

                var user = await userManager.FindByIdAsync(userId.ToString());
                
                if (user == null)
                {
                    return Json(new { success = false, message = $"Kullanıcı bulunamadı: {userId}" });
                }
             
                var currentRoles = await userManager.GetRolesAsync(user);
                var selectedRoles = roles?.Where(r => r.IsSelected && !string.IsNullOrEmpty(r.RoleName))
                                         .Select(r => r.RoleName).ToList() ?? new List<string>();

                // Mevcut rolleri kaldır
                if (currentRoles.Any())
                {
                    await userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                // Yeni rolleri ekle
                if (selectedRoles.Any())
                {
                    await userManager.AddToRolesAsync(user, selectedRoles);
                }
                else
                {
                    // Hiç rol seçilmemişse default member rolü ver
                    await userManager.AddToRoleAsync(user, "member");
                }

                return Json(new { success = true, message = "Kullanıcı rolleri başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        } 
    }
}
