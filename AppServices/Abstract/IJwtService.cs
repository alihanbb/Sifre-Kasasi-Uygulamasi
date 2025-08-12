using AppRepository.Entities;

namespace AppServices.Abstract
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(AppUser user);
    }
}
