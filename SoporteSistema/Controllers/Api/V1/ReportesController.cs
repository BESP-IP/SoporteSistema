using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteSistema.Contracts.Reportes;
using SoporteSistema.Models;
using SoporteSistema.Repositories;
using SoporteSistema.Security;

namespace SoporteSistema.Controllers.Api.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/reportes")]
[Authorize(Roles = AppRoles.Administrador + "," + AppRoles.Usuario)]
[Produces("application/json")]
public sealed class ReportesController(IReporteRepository reportes) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ReporteResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReporteResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var resultado = await reportes.GetAllAsync(cancellationToken);
        return Ok(resultado.Select(ToResponse).ToList());
    }

    [HttpGet("{id:int}", Name = "ObtenerReporteV1")]
    [ProducesResponseType(typeof(ReporteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReporteResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var reporte = await reportes.GetByIdAsync(id, cancellationToken);
        return reporte is null ? NotFound() : Ok(ToResponse(reporte));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReporteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReporteResponse>> Create(
        CrearReporteRequest request,
        CancellationToken cancellationToken)
    {
        // La categoría debe existir antes de guardar el reporte.
        if (!await reportes.CategoryExistsAsync(request.IdCategoria, cancellationToken))
        {
            ModelState.AddModelError(nameof(request.IdCategoria), "La categoría indicada no existe.");
            return ValidationProblem(ModelState);
        }

        var reporte = new Reporte
        {
            TituloReporte = request.TituloReporte.Trim(),
            TipoReporte = request.TipoReporte.Trim(),
            IdCategoria = request.IdCategoria,
            ArchivoUrl = request.ArchivoUrl?.Trim() ?? string.Empty
        };

        await reportes.AddAsync(reporte, cancellationToken);
        await reportes.SaveChangesAsync(cancellationToken);

        return CreatedAtRoute("ObtenerReporteV1", new { id = reporte.IdReporte, version = "1" }, ToResponse(reporte));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Administrador)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        int id,
        ActualizarReporteRequest request,
        CancellationToken cancellationToken)
    {
        var reporte = await reportes.GetByIdAsync(id, cancellationToken);
        if (reporte is null)
        {
            return NotFound();
        }

        if (!await reportes.CategoryExistsAsync(request.IdCategoria, cancellationToken))
        {
            ModelState.AddModelError(nameof(request.IdCategoria), "La categoría indicada no existe.");
            return ValidationProblem(ModelState);
        }

        reporte.TituloReporte = request.TituloReporte.Trim();
        reporte.TipoReporte = request.TipoReporte.Trim();
        reporte.IdCategoria = request.IdCategoria;
        reporte.ArchivoUrl = request.ArchivoUrl?.Trim() ?? string.Empty;

        await reportes.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Administrador)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var reporte = await reportes.GetByIdAsync(id, cancellationToken);
        if (reporte is null)
        {
            return NotFound();
        }

        await reportes.DeleteAsync(reporte, cancellationToken);
        try
        {
            await reportes.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            // No se puede borrar si hay comentarios o historial asociados.
            return Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "No se puede eliminar el reporte porque tiene comentarios o historial asociados.");
        }

        return NoContent();
    }

    // Solo enviamos los campos que necesita la API.
    private static ReporteResponse ToResponse(Reporte reporte) => new(
        reporte.IdReporte,
        reporte.TituloReporte,
        reporte.TipoReporte,
        reporte.IdCategoria,
        reporte.IdCategoriaNavigation.NombreCategoria,
        string.IsNullOrWhiteSpace(reporte.ArchivoUrl) ? null : reporte.ArchivoUrl);
}
