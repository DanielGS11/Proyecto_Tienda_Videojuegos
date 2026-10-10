using Microsoft.AspNetCore.Mvc;
using Proyecto_Tienda_Videojuegos.Context;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/others")]
[ApiController]
public class OtherController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public OtherController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetProducts")]
    public async Task<IActionResult> GetProducts()
    {
        return Ok();
    }
}