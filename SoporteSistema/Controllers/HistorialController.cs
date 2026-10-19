
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteSistema.Models;

public class HistorialController : Controller
{
    private readonly ApplicationDbContext _context;

    public HistorialController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: HISTORIALS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Historials.ToListAsync());
    }

    // GET: HISTORIALS/Details/5
    public async Task<IActionResult> Details(int? idhistorial)
    {
        if (idhistorial == null)
        {
            return NotFound();
        }

        var historial = await _context.Historials
            .FirstOrDefaultAsync(m => m.IdHistorial == idhistorial);
        if (historial == null)
        {
            return NotFound();
        }

        return View(historial);
    }

    // GET: HISTORIALS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: HISTORIALS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdHistorial,IdUsuario,IdReporte,Accion,Fecha,IdReporteNavigation,IdUsuarioNavigation")] Historial historial)
    {
        if (ModelState.IsValid)
        {
            _context.Add(historial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(historial);
    }

    // GET: HISTORIALS/Edit/5
    public async Task<IActionResult> Edit(int? idhistorial)
    {
        if (idhistorial == null)
        {
            return NotFound();
        }

        var historial = await _context.Historials.FindAsync(idhistorial);
        if (historial == null)
        {
            return NotFound();
        }
        return View(historial);
    }

    // POST: HISTORIALS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idhistorial, [Bind("IdHistorial,IdUsuario,IdReporte,Accion,Fecha,IdReporteNavigation,IdUsuarioNavigation")] Historial historial)
    {
        if (idhistorial != historial.IdHistorial)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(historial);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorialExists(historial.IdHistorial))
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
        return View(historial);
    }

    // GET: HISTORIALS/Delete/5
    public async Task<IActionResult> Delete(int? idhistorial)
    {
        if (idhistorial == null)
        {
            return NotFound();
        }

        var historial = await _context.Historials
            .FirstOrDefaultAsync(m => m.IdHistorial == idhistorial);
        if (historial == null)
        {
            return NotFound();
        }

        return View(historial);
    }

    // POST: HISTORIALS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idhistorial)
    {
        var historial = await _context.Historials.FindAsync(idhistorial);
        if (historial != null)
        {
            _context.Historials.Remove(historial);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HistorialExists(int? idhistorial)
    {
        return _context.Historials.Any(e => e.IdHistorial == idhistorial);
    }
}
