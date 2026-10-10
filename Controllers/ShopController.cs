using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.Entities;

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

    [HttpPost("CreateShop")]
    public async Task<IActionResult> CreateShop(string name)
    {
        var nameExists = await _context.Shops.AnyAsync(s => s.Name.ToLower() == name.ToLower());

        if (nameExists)
            return BadRequest("The shop already exists");

        _context.Shops.Add(new Shop { Name = name });
        await _context.SaveChangesAsync();

        return Ok();
    }
}