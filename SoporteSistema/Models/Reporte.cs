using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Models;

public partial class Reporte
{
    [Key]
    public int IdReporte { get; set; }

    public string TituloReporte { get; set; } = null!;

    public string TipoReporte { get; set; } = null!;

    public int IdCategoria { get; set; }

    public string ArchivoUrl { get; set; } = null!;

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual ICollection<Historial> Historials { get; set; } = new List<Historial>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;
}
