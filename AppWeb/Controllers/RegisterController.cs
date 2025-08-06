using AppRepository.Entities;
using AppServices.Abstract;
using AppWeb.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterService registerServices;
        private readonly ILogger<RegisterController> logger;
        private readonly UserManager<AppUser> userManager;
        
        public RegisterController(IRegisterService registeredServices, ILogger<RegisterController> logger, UserManager<AppUser> userManager)
        {
            this.registerServices = registeredServices;
            this.logger = logger;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            
            if (await registerServices.IsValidAsync(register.UserName))
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı zaten kayıtlı.");
                return View(register);
            }

            AppUser appuser = new AppUser
            {
                UserName = register.UserName,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName
            };
            var result = await userManager.CreateAsync(appuser, register.Password);

            if (result.Succeeded)
            {
                // Kullanıcıya "member" rolünü ata
                await userManager.AddToRoleAsync(appuser, "member");
                
                logger.LogInformation("Kullanıcı başarıyla kaydedildi: {UserName}", register.UserName);
                return RedirectToAction("Index", "Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
           
            return View(register);
        }
    }
}
