using SoporteSistema.Models;

namespace SoporteSistema.Repositories;

public interface IReporteRepository
{
    Task<IReadOnlyList<Reporte>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Reporte?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken = default);

    Task AddAsync(Reporte reporte, CancellationToken cancellationToken = default);

    Task DeleteAsync(Reporte reporte, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
