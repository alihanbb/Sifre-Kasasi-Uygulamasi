using Microsoft.AspNetCore.Identity;

namespace AppRepository.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Member> Members { get; set; }
    }
}
