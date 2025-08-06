using AppServices.Abstract;
using AppWeb.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService loginService;
        public LoginController(ILoginService loginService)
        {
                this.loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var user = await loginService.GetUserByUsernameAsync(model.UserName);


            if (user == null || !await loginService.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                return View();
            }
            await loginService.SignInAsync(user);

            return RedirectToAction("Index", "AdminMember");
        }
    }
}
