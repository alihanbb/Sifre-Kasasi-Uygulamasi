using AppRepository.Entities;

namespace AppWeb.ViewModel
{
    public class RoleViewModel
    {
        public AppUser User { get; set; }
        public List<string> Roles { get; internal set; }
    }
}
