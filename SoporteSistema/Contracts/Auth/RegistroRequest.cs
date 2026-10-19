using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Contracts.Auth;

public sealed class RegistroRequest
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string NombreUsuario { get; init; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [StringLength(150)]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 12, ErrorMessage = "La contraseña debe tener entre 12 y 100 caracteres.")]
    public string Contraseña { get; init; } = string.Empty;
}
