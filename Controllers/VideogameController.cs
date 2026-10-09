using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/videogames")]
[ApiController]
public class VideogameController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public VideogameController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetVideogames")]
    public async Task<IActionResult> GetVideogames()
    {
        var result = await _context.Videogames.Select(v => new
        {
            Id = v.Id,
            Name = v.Name,
            Genres = v.Genres.ToList(),
            Platforms = v.Platforms.Select(p => p.Name).ToList()

        }).ToListAsync();

        return Ok(result);
    }
}
