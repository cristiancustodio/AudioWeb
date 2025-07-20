using Microsoft.AspNetCore.Identity;

namespace AudioWeb.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Descricao { get; set; } = string.Empty;
    }
}