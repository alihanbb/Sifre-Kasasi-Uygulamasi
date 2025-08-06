using AppRepository.Entities;
using AppWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppWeb.Controllers
{
    [Authorize]
    public class AdminManagerController : Controller
    {
       
        private readonly UserManager<AppUser> userManager;
        public AdminManagerController(UserManager<AppUser> userManager)
        {
                this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await userManager.Users.ToListAsync();
            
            var userViewModels = new List<RoleViewModel>();
            
            foreach (var appUser in user)
            {
                var roles = await userManager.GetRolesAsync(appUser);
                
                userViewModels.Add(new RoleViewModel
                {
                    User = appUser,
                    Roles = roles.ToList()
                });
            }

            return View(userViewModels);
        }

    }
}
