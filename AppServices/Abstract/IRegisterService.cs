using AppRepository.Entities;
using Microsoft.AspNetCore.Identity;

namespace AppServices.Abstract
{
    public interface IRegisterService
    {

        Task<bool> IsValidAsync(string userName);
    }
}
