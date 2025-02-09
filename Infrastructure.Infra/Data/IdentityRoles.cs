using Domain.DTO.Autenticacao;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data;

public class IdentityRoles
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in GetAllRoles())
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static List<string> GetAllRoles()
    {
        return new List<string>
    {
        nameof(UserRoles.Medico),
        nameof(UserRoles.Paciente)
    };
    }
}