using Microsoft.AspNetCore.Identity;

namespace Lab11_ASP_Identity.Data.Models
{
    public class Role:IdentityRole<int>
    {
        public string RoleName { get; set; } = null!;
    }
}
