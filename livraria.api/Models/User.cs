using livraria.api.Enums;
using Microsoft.AspNetCore.Identity;

namespace livraria.api.Models
{
    public class User: IdentityUser<Guid>
    {
        public bool Active { get; set; }
        public string FullName { get; set; } = string.Empty;
        public EUserType UserType { get; set; }
    }
}
