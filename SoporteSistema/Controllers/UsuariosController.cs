
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteSistema.Contracts.Usuarios;
using SoporteSistema.Models;
using SoporteSistema.Security;

public class UsuariosController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordService _passwords;

    public UsuariosController(ApplicationDbContext context, IPasswordService passwords)
    {
        _context = context;
        _passwords = passwords;
    }

    // GET: USUARIOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Usuarios.ToListAsync());
    }

    // GET: USUARIOS/Details/5
    public async Task<IActionResult> Details(int? idusuario)
    {
        if (idusuario == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(m => m.IdUsuario == idusuario);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // GET: USUARIOS/Create
    public IActionResult Create()
    {
        return View(new UsuarioFormulario());
    }

    // POST: USUARIOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioFormulario formulario)
    {
        if (string.IsNullOrWhiteSpace(formulario.Contraseña) || formulario.Contraseña.Length < 12)
        {
            ModelState.AddModelError(nameof(formulario.Contraseña), "La contraseña debe tener al menos 12 caracteres.");
        }

        var email = formulario.Email.Trim();
        if (await _context.Usuarios.AnyAsync(usuario => usuario.Email == email))
        {
            ModelState.AddModelError(nameof(formulario.Email), "Ya existe una cuenta con ese correo.");
        }

        if (ModelState.IsValid)
        {
            var usuario = new Usuario
            {
                NombreUsuario = formulario.NombreUsuario.Trim(),
                Email = email,
                Rol = AppRoles.Usuario,
                Contraseña = string.Empty
            };
            usuario.Contraseña = _passwords.Hash(usuario, formulario.Contraseña!);
            _context.Add(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(formulario);
    }

    // GET: USUARIOS/Edit/5
    public async Task<IActionResult> Edit(int? idusuario)
    {
        if (idusuario == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios.FindAsync(idusuario);
        if (usuario == null)
        {
            return NotFound();
        }
        return View(new UsuarioFormulario
        {
            IdUsuario = usuario.IdUsuario,
            NombreUsuario = usuario.NombreUsuario,
            Email = usuario.Email
        });
    }

    // POST: USUARIOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? idusuario, UsuarioFormulario formulario)
    {
        if (idusuario != formulario.IdUsuario)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios.FindAsync(idusuario);
        if (usuario is null)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(formulario.Contraseña) && formulario.Contraseña.Length < 12)
        {
            ModelState.AddModelError(nameof(formulario.Contraseña), "La contraseña debe tener al menos 12 caracteres.");
        }

        var email = formulario.Email.Trim();
        if (await _context.Usuarios.AnyAsync(otro => otro.IdUsuario != usuario.IdUsuario && otro.Email == email))
        {
            ModelState.AddModelError(nameof(formulario.Email), "Ya existe una cuenta con ese correo.");
        }

        if (ModelState.IsValid)
        {
            usuario.NombreUsuario = formulario.NombreUsuario.Trim();
            usuario.Email = email;
            if (!string.IsNullOrWhiteSpace(formulario.Contraseña))
            {
                usuario.Contraseña = _passwords.Hash(usuario, formulario.Contraseña);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(formulario);
    }

    // GET: USUARIOS/Delete/5
    public async Task<IActionResult> Delete(int? idusuario)
    {
        if (idusuario == null)
        {
            return NotFound();
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(m => m.IdUsuario == idusuario);
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    // POST: USUARIOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? idusuario)
    {
        var usuario = await _context.Usuarios.FindAsync(idusuario);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}
