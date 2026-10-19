using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Models;

public partial class Categoria
{
    [Key]
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();
}
