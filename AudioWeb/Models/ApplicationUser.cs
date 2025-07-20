using Microsoft.AspNetCore.Identity;

namespace AudioWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
    }
}