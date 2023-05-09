using Lab11_ASP_Identity.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab11_ASP_Identity.Data
{
    public class ASPIdentityContext:IdentityDbContext<User,Role,int>
    {
        public ASPIdentityContext(DbContextOptions<ASPIdentityContext> options):base(options)
        {

        }
    }
}
