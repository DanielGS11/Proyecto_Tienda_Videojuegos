using Microsoft.AspNetCore.Mvc;
using Proyecto_Tienda_Videojuegos.Context;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/cities")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public CityController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetCities")]
    public async Task<IActionResult> GetCities()
    {
        return Ok();
    }
}