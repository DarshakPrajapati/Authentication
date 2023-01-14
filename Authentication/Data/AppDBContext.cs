using Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data
{
    public class AppDBContext : IdentityDbContext<AppUser, IdentityRole, String>
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
    }
}
