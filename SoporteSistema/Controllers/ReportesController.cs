
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteSistema.Models;

public class ReportesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: REPORTES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Reportes.ToListAsync());
    }

    // GET: REPORTES/Details/5
    public async Task<IActionResult> Details(int? idreporte)
    {
        if (idreporte == null)
        {
            return NotFound();
        }

        var reporte = await _context.Reportes
            .FirstOrDefaultAsync(m => m.IdReporte == idreporte);
        if (reporte == null)
        {
            return NotFound();
        }

        return View(reporte);
    }

    // GET: REPORTES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: REPORTES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdReporte,TituloReporte,TipoReporte,IdCategoria,ArchivoUrl,Comentarios,Historials,IdCategoriaNavigation")] Reporte reporte)
    {
        if (ModelState.IsValid)
        {
            _context.Add(reporte);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(reporte);
    }

    // GET: REPORTES/Edit/5
    public async Task<IActionResult> Edit(int? idreporte)
    {
        if (idreporte == null)
        {
            return NotFound();
        }

        var reporte = await _context.Reportes.FindAsync(idreporte);
        if (reporte == null)
        {
            return NotFound();
        }
        return View(reporte);
    }

    // POST: REPORTES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idreporte, [Bind("IdReporte,TituloReporte,TipoReporte,IdCategoria,ArchivoUrl,Comentarios,Historials,IdCategoriaNavigation")] Reporte reporte)
    {
        if (idreporte != reporte.IdReporte)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(reporte);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReporteExists(reporte.IdReporte))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(reporte);
    }

    // GET: REPORTES/Delete/5
    public async Task<IActionResult> Delete(int? idreporte)
    {
        if (idreporte == null)
        {
            return NotFound();
        }

        var reporte = await _context.Reportes
            .FirstOrDefaultAsync(m => m.IdReporte == idreporte);
        if (reporte == null)
        {
            return NotFound();
        }

        return View(reporte);
    }

    // POST: REPORTES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idreporte)
    {
        var reporte = await _context.Reportes.FindAsync(idreporte);
        if (reporte != null)
        {
            _context.Reportes.Remove(reporte);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReporteExists(int? idreporte)
    {
        return _context.Reportes.Any(e => e.IdReporte == idreporte);
    }
}
