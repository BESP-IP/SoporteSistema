
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteSistema.Models;

public class CategoriasController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CATEGORIAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Categoria.ToListAsync());
    }

    // GET: CATEGORIAS/Details/5
    public async Task<IActionResult> Details(int? idcategoria)
    {
        if (idcategoria == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(m => m.IdCategoria == idcategoria);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // GET: CATEGORIAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CATEGORIAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("IdCategoria,NombreCategoria,Descripcion,Reportes")] Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    // GET: CATEGORIAS/Edit/5
    public async Task<IActionResult> Edit(int? idcategoria)
    {
        if (idcategoria == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria.FindAsync(idcategoria);
        if (categoria == null)
        {
            return NotFound();
        }
        return View(categoria);
    }

    // POST: CATEGORIAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idcategoria, [Bind("IdCategoria,NombreCategoria,Descripcion,Reportes")] Categoria categoria)
    {
        if (idcategoria != categoria.IdCategoria)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoria.IdCategoria))
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
        return View(categoria);
    }

    // GET: CATEGORIAS/Delete/5
    public async Task<IActionResult> Delete(int? idcategoria)
    {
        if (idcategoria == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(m => m.IdCategoria == idcategoria);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // POST: CATEGORIAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idcategoria)
    {
        var categoria = await _context.Categoria.FindAsync(idcategoria);
        if (categoria != null)
        {
            _context.Categoria.Remove(categoria);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoriaExists(int? idcategoria)
    {
        return _context.Categoria.Any(e => e.IdCategoria == idcategoria);
    }
}
