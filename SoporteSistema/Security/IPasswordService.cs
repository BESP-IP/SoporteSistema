using SoporteSistema.Models;

namespace SoporteSistema.Security;

public interface IPasswordService
{
    string Hash(Usuario usuario, string password);

    Task<bool> VerifyAndUpgradeAsync(Usuario usuario, string password, CancellationToken cancellationToken = default);
}
