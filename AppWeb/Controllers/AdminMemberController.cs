using AppRepository.Entities;
using AppRepository.Entities.Dtos;
using AppServices.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    [Authorize]
    public class AdminMemberController : Controller
    {
        private readonly IMemberService memberService;
        private readonly UserManager<AppUser> userManager;
        public AdminMemberController(IMemberService memberService, UserManager<AppUser> userManager)
        {
            this.memberService = memberService;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Members()
        {
            var user = await userManager.GetUserAsync(User);
            var members = await memberService.GetAllAsync(user.Id);
            return View(members);
        }

        [HttpGet]
        public IActionResult CreateMemberAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMemberAdmin(CreateMemberDto createUser)
        {
            var user = await userManager.GetUserAsync(User);
            await memberService.CreateAsync(createUser, user.Id);
            return RedirectToAction("Members");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMemberAdmin(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var member = await memberService.GetByIdAsync(id, user.Id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMemberAdmin(UpdateMemberDto updateUser)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                await memberService.UpdatesAsync(updateUser,user.Id);
                return RedirectToAction(nameof(Members));          
            }
            return View(updateUser);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMemberAdmin(int id)
        {
            var user = await userManager.GetUserAsync(User);
            await memberService.DeleteAsync(id, user.Id);
            return RedirectToAction(nameof(Members));
        }
    }
}
