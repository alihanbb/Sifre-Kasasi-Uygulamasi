using AppRepository.Entities;

namespace AppWeb.ViewModel
{
    public class RoleViewModel
    {
        public AppUser User { get; set; }  
        public string RoleName { get; set; }
        
        public List<string> Roles { get; set; } 
        public int RoleId { get; set; }
        public bool IsSelected { get; set; } 
    }
}
