using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoporteSistema.Contracts.Auth;
using SoporteSistema.Models;
using SoporteSistema.Security;

namespace SoporteSistema.Controllers.Api.V1;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/auth")]
[Produces("application/json")]
public sealed class AuthController(
    ApplicationDbContext dbContext,
    IPasswordService passwords,
    IJwtTokenService tokens) : ControllerBase
{
    // El registro público siempre crea cuentas con rol Usuario.
    [AllowAnonymous]
    [HttpPost("registro")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(RegistroRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        if (await dbContext.Usuarios.AnyAsync(usuario => usuario.Email == email, cancellationToken))
        {
            return Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Ya existe una cuenta con ese correo.");
        }

        var usuario = new Usuario
        {
            NombreUsuario = request.NombreUsuario.Trim(),
            Email = email,
            Rol = AppRoles.Usuario,
            Contraseña = string.Empty
        };
        usuario.Contraseña = passwords.Hash(usuario, request.Contraseña);

        dbContext.Usuarios.Add(usuario);
        await dbContext.SaveChangesAsync(cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new
        {
            usuario.IdUsuario,
            usuario.NombreUsuario,
            usuario.Email,
            usuario.Rol
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TokenResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var usuario = await dbContext.Usuarios
            .FirstOrDefaultAsync(cuenta => cuenta.Email == email, cancellationToken);

        if (usuario is null || !await passwords.VerifyAndUpgradeAsync(usuario, request.Contraseña, cancellationToken))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Credenciales no válidas."
            });
        }

        if (usuario.Rol is not (AppRoles.Administrador or AppRoles.Usuario))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "La cuenta no tiene un rol válido."
            });
        }

        return Ok(tokens.CreateToken(usuario));
    }
}
