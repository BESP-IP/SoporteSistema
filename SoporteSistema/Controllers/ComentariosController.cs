
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteSistema.Models;

public class ComentariosController : Controller
{
    private readonly ApplicationDbContext _context;

    public ComentariosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: COMENTARIOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Comentarios.ToListAsync());
    }

    // GET: COMENTARIOS/Details/5
    public async Task<IActionResult> Details(int? idcomentario)
    {
        if (idcomentario == null)
        {
            return NotFound();
        }

        var comentario = await _context.Comentarios
            .FirstOrDefaultAsync(m => m.IdComentario == idcomentario);
        if (comentario == null)
        {
            return NotFound();
        }

        return View(comentario);
    }

    // GET: COMENTARIOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: COMENTARIOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdComentario,IdReporte,IdUsuario,Texto,Fecha,IdReporteNavigation,IdUsuarioNavigation")] Comentario comentario)
    {
        if (ModelState.IsValid)
        {
            _context.Add(comentario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(comentario);
    }

    // GET: COMENTARIOS/Edit/5
    public async Task<IActionResult> Edit(int? idcomentario)
    {
        if (idcomentario == null)
        {
            return NotFound();
        }

        var comentario = await _context.Comentarios.FindAsync(idcomentario);
        if (comentario == null)
        {
            return NotFound();
        }
        return View(comentario);
    }

    // POST: COMENTARIOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idcomentario, [Bind("IdComentario,IdReporte,IdUsuario,Texto,Fecha,IdReporteNavigation,IdUsuarioNavigation")] Comentario comentario)
    {
        if (idcomentario != comentario.IdComentario)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(comentario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(comentario.IdComentario))
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
        return View(comentario);
    }

    // GET: COMENTARIOS/Delete/5
    public async Task<IActionResult> Delete(int? idcomentario)
    {
        if (idcomentario == null)
        {
            return NotFound();
        }

        var comentario = await _context.Comentarios
            .FirstOrDefaultAsync(m => m.IdComentario == idcomentario);
        if (comentario == null)
        {
            return NotFound();
        }

        return View(comentario);
    }

    // POST: COMENTARIOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idcomentario)
    {
        var comentario = await _context.Comentarios.FindAsync(idcomentario);
        if (comentario != null)
        {
            _context.Comentarios.Remove(comentario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ComentarioExists(int? idcomentario)
    {
        return _context.Comentarios.Any(e => e.IdComentario == idcomentario);
    }
}
