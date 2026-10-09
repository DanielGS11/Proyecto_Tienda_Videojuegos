using Microsoft.AspNetCore.Mvc;
using Proyecto_Tienda_Videojuegos.Context;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/shops")]
[ApiController]
public class ShopController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public ShopController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetShops")]
    public async Task<IActionResult> GetShops()
    {
        return Ok();
    }
}