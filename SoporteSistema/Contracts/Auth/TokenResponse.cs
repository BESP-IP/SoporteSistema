namespace SoporteSistema.Contracts.Auth;

public sealed record TokenResponse(
    string AccessToken,
    string TokenType,
    DateTime ExpiresAtUtc,
    string NombreUsuario,
    string Rol);
