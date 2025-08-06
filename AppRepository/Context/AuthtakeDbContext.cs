using AppRepository.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppRepository.Context
{
    public class AuthtakeDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AuthtakeDbContext(DbContextOptions<AuthtakeDbContext> options) : base(options) { }

        public DbSet<Member> Members { get; set; }
    }
}
