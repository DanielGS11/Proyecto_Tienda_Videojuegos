using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.Entities;

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
        var result = await _context.Providers.Select(p => new
        {
            Id = p.Id,
            Name = p.Name,
            Cities = p.Cities.Select(c => c.Name).ToList(),
            Products = p.Products.Select(pr => new
            {
                Name = pr.Name,
                Price = $"{pr.Price:C}"
            }).ToList()
        }).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetProvider/{id}")]
    public async Task<IActionResult> GetProvider(int id)
    {
        var result = await _context.Providers.Where(p => p.Id == id).Select(p => new
        {
            Id = p.Id,
            Name = p.Name,
            Cities = p.Cities.Select(c => c.Name).ToList(),
            Products = p.Products.Select(pr => new
            {
                Name = pr.Name,
                Price = $"{pr.Price:C}"
            }).ToList()
        }).FirstOrDefaultAsync();

        if (result is null) 
            return NotFound();

        return Ok(result);
    }

    [HttpPost("CreateProvider")]
    public async Task<IActionResult> CreateProvider(string name)
    {
        var nameExists = await _context.Providers.AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim());

        if (nameExists) 
            return BadRequest("There is already a provider with that name");
        
        _context.Providers.Add(new Provider{Name = name});
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("AddProductToProvider/product/{productId}/provider/{providerId}")]
    public async Task<IActionResult> AddProductToProvider(int productId, int providerId)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product is null)
            return NotFound("The product does not exists");

        var provider = await _context.Providers.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == providerId);

        if (provider is null)
            return NotFound("The provider does not exists");
        
        if (provider.Products.Contains(product))
            return BadRequest("The provider already has that product");

        provider.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("AddCityToProvider/city/{cityId}/provider/{providerId}")]
    public async Task<IActionResult> AddCityToProvider(int cityId, int providerId)
    {
        var city = await _context.Cities.FindAsync(cityId);

        if (city is null)
            return NotFound("The city does not exists");

        var provider = await _context.Providers.Include(p => p.Cities).FirstOrDefaultAsync(p => p.Id == providerId);

        if (provider is null)
            return NotFound("The provider does not exists");
        
        if (provider.Cities.Contains(city))
            return BadRequest("The provider already delivers to that city");

        provider.Cities.Add(city);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("UpdateProvider/{id}")]
    public async Task<IActionResult> UpdateProvider(int id, string newName)
    {
        var nameExists = await _context.Providers.AnyAsync(p => p.Name.ToLower().Trim() == newName.ToLower().Trim());

        if (nameExists) 
            return BadRequest("There is already a provider with that name");
        
        var rowsAffected = await _context.Providers.Where(p => p.Id == id).ExecuteUpdateAsync(p =>
            p.SetProperty(p => p.Name, newName));
        
        if (rowsAffected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("DeleteProvider/{id}")]
    public async Task<IActionResult> DeleteProvider(int id)
    {
        var rowsAffected = await _context.Providers.Where(p => p.Id == id).ExecuteDeleteAsync();
        
        if (rowsAffected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("RemoveProductFromProvider/product/{productId}/provider/{providerId}")]
    public async Task<IActionResult> RemoveProductFromProvider(int productId, int providerId)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product is null)
            return NotFound("The product does not exists");

        var provider = await _context.Providers.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == providerId);

        if (provider is null)
            return NotFound("The provider does not exists");
        
        if (!provider.Products.Contains(product))
            return BadRequest("The provider does not have that product");

        provider.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("RemoveCityFromProvider/city/{cityId}/provider/{providerId}")]
    public async Task<IActionResult> RemoveCityFromProvider(int cityId, int providerId)
    {
        var city = await _context.Cities.FindAsync(cityId);

        if (city is null)
            return NotFound("The city does not exists");

        var provider = await _context.Providers.Include(p => p.Cities).FirstOrDefaultAsync(p => p.Id == providerId);

        if (provider is null)
            return NotFound("The provider does not exists");
        
        if (!provider.Cities.Contains(city))
            return BadRequest("The provider does not deliver to that city");

        provider.Cities.Remove(city);
        await _context.SaveChangesAsync();

        return Ok();
    }
}