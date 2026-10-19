using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Contracts.Auth;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Contraseña { get; init; } = string.Empty;
}
