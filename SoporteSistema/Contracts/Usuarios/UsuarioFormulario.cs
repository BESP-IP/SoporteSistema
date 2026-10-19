using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Contracts.Usuarios;

public sealed class UsuarioFormulario
{
    public int IdUsuario { get; init; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "La contraseña no puede superar los 100 caracteres.")]
    [DataType(DataType.Password)]
    public string? Contraseña { get; set; }
}
