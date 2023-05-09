using Microsoft.AspNetCore.Identity;

namespace Lab11_ASP_Identity.Data.Models
{
    public class User:IdentityUser<int>
    {
        public string Name { get; set; } = null!;
    }
}
