using livraria.api.Enums;
using livraria.api.Models;
using Microsoft.AspNetCore.Identity;

namespace livraria.api.Data
{
    public class DataInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager)
        {
            var admin = new User
            {
                Id = Guid.NewGuid(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = "admin@infinito.com",
                PhoneNumber = "925378361",
                NormalizedUserName = "Administrador",
                NormalizedEmail = "admin@infininto.com",
                FullName = "Administrador",
                UserName = "infinito",
                EmailConfirmed = true,
                UserType = EUserType.Administrator,
                Active = true
            };

            // Verifica se o usuário já existe
            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                var result = await userManager.CreateAsync(admin, "123@Mudar");
                if (!result.Succeeded)
                {
                    throw new Exception($"Erro ao criar usuário: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
