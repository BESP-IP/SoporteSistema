using Microsoft.EntityFrameworkCore;
using SoporteSistema.Models;

namespace SoporteSistema.Repositories;

public sealed class ReporteRepository(ApplicationDbContext dbContext) : IReporteRepository
{
    public async Task<IReadOnlyList<Reporte>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Reportes
            .AsNoTracking()
            .Include(reporte => reporte.IdCategoriaNavigation)
            .OrderBy(reporte => reporte.IdReporte)
            .ToListAsync(cancellationToken);

    public Task<Reporte?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        dbContext.Reportes
            .Include(reporte => reporte.IdCategoriaNavigation)
            .FirstOrDefaultAsync(reporte => reporte.IdReporte == id, cancellationToken);

    public Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken = default) =>
        dbContext.Categoria.AnyAsync(categoria => categoria.IdCategoria == categoryId, cancellationToken);

    public async Task AddAsync(Reporte reporte, CancellationToken cancellationToken = default) =>
        await dbContext.Reportes.AddAsync(reporte, cancellationToken);

    public Task DeleteAsync(Reporte reporte, CancellationToken cancellationToken = default)
    {
        dbContext.Reportes.Remove(reporte);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
