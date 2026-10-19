using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Models;

public partial class Historial
{
    [Key]
    public int IdHistorial { get; set; }

    public int IdUsuario { get; set; }

    public int IdReporte { get; set; }

    public string Accion { get; set; } = null!;

    public DateOnly? Fecha { get; set; }

    public virtual Reporte IdReporteNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
