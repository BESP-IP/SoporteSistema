using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SoporteSistema.Models;

namespace SoporteSistema.Security;

public sealed class PasswordService(
    IPasswordHasher<Usuario> passwordHasher,
    ApplicationDbContext dbContext) : IPasswordService
{
    public string Hash(Usuario usuario, string password) =>
        passwordHasher.HashPassword(usuario, password);

    public async Task<bool> VerifyAndUpgradeAsync(
        Usuario usuario,
        string password,
        CancellationToken cancellationToken = default)
    {
        var result = passwordHasher.VerifyHashedPassword(usuario, usuario.Contraseña, password);
        if (result != PasswordVerificationResult.Failed)
        {
            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                usuario.Contraseña = Hash(usuario, password);
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return true;
        }

        // Convierte las contraseñas antiguas en texto claro al primer inicio de sesión correcto
        var storedBytes = Encoding.UTF8.GetBytes(usuario.Contraseña);
        var suppliedBytes = Encoding.UTF8.GetBytes(password);
        if (storedBytes.Length != suppliedBytes.Length ||
            !CryptographicOperations.FixedTimeEquals(storedBytes, suppliedBytes))
        {
            return false;
        }

        usuario.Contraseña = Hash(usuario, password);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
