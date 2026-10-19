using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Models;

public partial class Comentario
{
    [Key]
    public int IdComentario { get; set; }

    public int IdReporte { get; set; }

    public int IdUsuario { get; set; }

    public string Texto { get; set; } = null!;

    public DateOnly? Fecha { get; set; }

    public virtual Reporte IdReporteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
