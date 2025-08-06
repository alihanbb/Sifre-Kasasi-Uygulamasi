using AppRepository.Entities;

namespace AppServices.Abstract
{
    public interface ILoginService
    {
        Task Logout();
        Task<AppUser?> GetUserByUsernameAsync(string username);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task SignInAsync(AppUser user);
    }
}
