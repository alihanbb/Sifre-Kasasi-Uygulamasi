using AppRepository.Entities;
using AppRepository.Entities.Dtos;
using AppServices.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    [Authorize(Roles ="manager,member")]
    public class AdminMemberController : Controller
    {
        private readonly IMemberService memberService;
        private readonly UserManager<AppUser> userManager;
        
        public AdminMemberController(IMemberService memberService, UserManager<AppUser> userManager)
        {
            this.memberService = memberService;
            this.userManager = userManager;
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
                var members = await memberService.GetAllAsync(user.Id);
                return Json(new { success = true, data = members });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetMemberJson(int id)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                var member = await memberService.GetByIdAsync(id, user.Id);
                
                if (member == null)
                    return Json(new { success = false, message = "Kayıt bulunamadı" });
                
                return Json(new { success = true, data = member });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CreateMemberJson([FromBody] CreateMemberDto createUser)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState });
            }

            try
            {
                var user = await userManager.GetUserAsync(User);
                await memberService.CreateAsync(createUser, user.Id);
                return Json(new { success = true, message = "Kullanıcı bilgileri başarıyla eklendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UpdateMemberJson([FromBody] UpdateMemberDto updateUser)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, errors = ModelState });
            }

            try
            {
                var user = await userManager.GetUserAsync(User);
                await memberService.UpdatesAsync(updateUser, user.Id);
                return Json(new { success = true, message = "Kullanıcı bilgileri başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteMemberJson(int id)
        {
            try
            {
                var user = await userManager.GetUserAsync(User);
                await memberService.DeleteAsync(id, user.Id);
                return Json(new { success = true, message = "Kullanıcı bilgileri başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
