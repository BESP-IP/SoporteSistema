using System.ComponentModel.DataAnnotations;

namespace SoporteSistema.Contracts.Reportes;

public sealed class CrearReporteRequest
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
    public string TituloReporte { get; init; } = string.Empty;

    [Required(ErrorMessage = "El tipo de reporte es obligatorio.")]
    [StringLength(50, ErrorMessage = "El tipo no puede superar los 50 caracteres.")]
    public string TipoReporte { get; init; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar una categoría válida.")]
    public int IdCategoria { get; init; }

    [StringLength(255, ErrorMessage = "La URL del archivo no puede superar los 255 caracteres.")]
    [Url(ErrorMessage = "La URL del archivo no es válida.")]
    public string? ArchivoUrl { get; init; }
}
