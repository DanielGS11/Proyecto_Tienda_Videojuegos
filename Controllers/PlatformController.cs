using Microsoft.AspNetCore.Mvc;
using Proyecto_Tienda_Videojuegos.Context;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/platforms")]
[ApiController]
public class PlatformController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public PlatformController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetPlatforms")]
    public async Task<IActionResult> GetPlatforms()
    {
        return Ok();
    }
}