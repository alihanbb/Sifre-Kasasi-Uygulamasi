using AppRepository.Entities;

namespace AppWeb.ViewModel
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }
        public AppUser User { get; set; } = new AppUser();
        public string RoleName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}