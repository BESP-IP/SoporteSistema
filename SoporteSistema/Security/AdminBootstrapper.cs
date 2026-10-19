using Microsoft.EntityFrameworkCore;
using SoporteSistema.Models;

namespace SoporteSistema.Security;

public static class AdminBootstrapper
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var email = configuration["BootstrapAdmin:Email"];
        var password = configuration["BootstrapAdmin:Password"];
        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || password.Length < 12)
        {
            throw new InvalidOperationException(
                "BootstrapAdmin:Email y BootstrapAdmin:Password deben configurarse juntos; la contraseña debe tener al menos 12 caracteres.");
        }

        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var normalizedEmail = email.Trim();
        if (await dbContext.Usuarios.AnyAsync(usuario => usuario.Email == normalizedEmail))
        {
            return;
        }

        var usuario = new Usuario
        {
            NombreUsuario = "Administrador",
            Email = normalizedEmail,
            Rol = AppRoles.Administrador,
            Contraseña = string.Empty
        };
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();
        usuario.Contraseña = passwordService.Hash(usuario, password);
        dbContext.Usuarios.Add(usuario);
        await dbContext.SaveChangesAsync();
    }
}
