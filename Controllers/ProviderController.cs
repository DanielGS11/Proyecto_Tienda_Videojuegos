using Microsoft.AspNetCore.Mvc;
using Proyecto_Tienda_Videojuegos.Context;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/providers")]
[ApiController]
public class ProviderController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public ProviderController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetProviders")]
    public async Task<IActionResult> GetProviders()
    {
        return Ok();
    }
}