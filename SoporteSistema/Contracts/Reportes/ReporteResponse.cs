namespace SoporteSistema.Contracts.Reportes;

public sealed record ReporteResponse(
    int IdReporte,
    string TituloReporte,
    string TipoReporte,
    int IdCategoria,
    string NombreCategoria,
    string? ArchivoUrl);
