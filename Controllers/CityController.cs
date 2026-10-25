using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.Entities;

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
    public async Task<IActionResult> GetCities(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");

        var result = await _context.Cities.Select(c => new
        {
            Id = c.Id,
            Name = c.Name,
            Shops = c.Shops.Select(s => s.Name).ToList()

        }).Skip((page - 1) * 10).Take(10).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetCity/{id}")]
    public async Task<IActionResult> GetCity(int id)
    {
        var result = await _context.Cities.Where(c => c.Id == id).Select(c => new
        {
            Id = c.Id,
            Name = c.Name,
            Shops = c.Shops.Select(s => s.Name).ToList()
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("CreateCity")]
    public async Task<IActionResult> CreateCity(string name)
    {
        var nameExists = await _context.Cities.AnyAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());

        if (nameExists)
            return BadRequest("The city already exists");

        _context.Cities.Add(new City { Name = name });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("AddShopToCity/city/{cityId}/shop/{shopId}")]
    public async Task<IActionResult> AddShopToCity(int cityId, int shopId)
    {
        var city = await _context.Cities.Include(c => c.Shops).FirstOrDefaultAsync(c => c.Id == cityId);

        if (city is null) 
            return NotFound("City not found");
        
        var shop = await _context.Shops.FindAsync(shopId);

        if (shop is null) 
            return NotFound("Shop not found");
        
        if (city.Shops.Contains(shop))
            return BadRequest("The shop was already in that city");

        city.Shops.Add(shop);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("UpdateCity/{id}")]
    public async Task<IActionResult> UpdateCity(int id, string newName)
    {
        var nameExists = await _context.Cities.AnyAsync(c => c.Name.ToLower().Trim() == newName.ToLower().Trim());

        if (nameExists)
            return BadRequest("There is already a city with that name");

        var rowsAfected = await _context.Cities.Where(c => c.Id == id)
            .ExecuteUpdateAsync(c => c.SetProperty(c => c.Name, newName));
        
        if (rowsAfected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("DeleteCity/{id}")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        var rowsAfected = await _context.Cities.Where(c => c.Id == id).ExecuteDeleteAsync();

        if (rowsAfected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("RemoveShopFromCity/city/{cityId}/shop/{shopId}")]
    public async Task<IActionResult> RemoveShopFromCity(int cityId, int shopId)
    {
        var city = await _context.Cities.Include(c => c.Shops).FirstOrDefaultAsync(c => c.Id == cityId);

        if (city is null) 
            return NotFound("City not found");
        
        var shop = await _context.Shops.FindAsync(shopId);

        if (shop is null) 
            return NotFound("Shop not found");
        
        if (!city.Shops.Contains(shop))
            return BadRequest("The city does not have that shop");

        city.Shops.Remove(shop);
        await _context.SaveChangesAsync();

        return Ok();
    }
}