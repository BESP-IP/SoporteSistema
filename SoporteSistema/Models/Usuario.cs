using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Models;

public partial class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Historial> Historials { get; set; } = new List<Historial>();
}
