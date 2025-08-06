using AppRepository.Entities;
using AppServices.Abstract;
using Microsoft.AspNetCore.Identity;

namespace AppServices.Concrete
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<AppUser> userManager;

        public RegisterService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<bool> IsValidAsync(string userName)
        {
            var  existUserName = await userManager.FindByNameAsync(userName);
            if (existUserName == null) 
                return false;
            return true;
        }
    }
}
