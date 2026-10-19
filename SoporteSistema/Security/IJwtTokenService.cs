using SoporteSistema.Contracts.Auth;
using SoporteSistema.Models;

namespace SoporteSistema.Security;

public interface IJwtTokenService
{
    TokenResponse CreateToken(Usuario usuario);
}
