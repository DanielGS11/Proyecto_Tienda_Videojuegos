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
    public async Task<IActionResult> GetShops(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");

        var result = await _context.Shops.Select(s => new
        {
            Id = s.Id,
            Name = s.Name,
            Cities = s.Cities.Select(c => c.Name).ToList(),
            Employees = s.Employees.Select(e => e.Name).ToList(),
            Providers = s.Providers.Select(p => p.Name).ToList(),
            Products = s.Products.Select(p => new 
            {
                Name = p.Name,
                Price = $"{p.Price}$"
            }).ToList()
        }).Skip((page - 1) * 10).Take(10).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetShop/{id}")]
    public async Task<IActionResult> GetShop(int id)
    {
        var result = await _context.Shops.Where(s => s.Id == id).Select(s => new
        {
            Id = s.Id,
            Name = s.Name,
            Cities = s.Cities.Select(c => c.Name).ToList(),
            Employees = s.Employees.Select(e => e.Name).ToList(),
            Providers = s.Providers.Select(p => p.Name).ToList(),
            Products = s.Products.Select(p => new
            {
                Name = p.Name,
                Price = $"{p.Price}$"
            }).ToList()
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("CreateShop")]
    public async Task<IActionResult> CreateShop(string name)
    {
        var nameExists = await _context.Shops.AnyAsync(s => s.Name.ToLower().Trim() == name.ToLower().Trim());

        if (nameExists)
            return BadRequest("The shop already exists");

        _context.Shops.Add(new Shop { Name = name });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("AddProductToShop/product/{productId}/shop/{shopId}")]
    public async Task<IActionResult> AddProductToShop(int productId, int shopId)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product is null) 
            return NotFound("The product does not exists");
        
        var shop = await _context.Shops.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == shopId);

        if (shop is null)
            return NotFound("The shop does not exists");
        
        if (shop.Products.Contains(product))
            return BadRequest("The shop already has that product");

        shop.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("AddProviderToShop/provider/{providerId}/shop/{shopId}")]
    public async Task<IActionResult> AddProviderToShop(int providerId, int shopId)
    {
        var provider = await _context.Providers.FindAsync(providerId);

        if (provider is null) 
            return NotFound("The provider does not exists");
        
        var shop = await _context.Shops.Include(s => s.Providers).FirstOrDefaultAsync(s => s.Id == shopId);

        if (shop is null)
            return NotFound("The shop does not exists");
        
        if (shop.Providers.Contains(provider))
            return BadRequest("The shop already has that provider");

        shop.Providers.Add(provider);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("UpdateShop/{id}")]
    public async Task<IActionResult> UpdateShop(int id, string newName)
    {
        var nameExists = await _context.Shops.AnyAsync(s => s.Name.ToLower().Trim() == newName.ToLower().Trim());

        if (nameExists)
            return BadRequest("There is already a shop with that name");

        var rowsAfected = await _context.Shops.Where(s => s.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(s => s.Name, newName));
        
        if (rowsAfected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("DeleteShop/{id}")]
    public async Task<IActionResult> DeleteShop(int id)
    {
        var rowsAfected = await _context.Shops.Where(s => s.Id == id).ExecuteDeleteAsync();

        if (rowsAfected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("RemoveProductFromShop/product/{productId}/shop/{shopId}")]
    public async Task<IActionResult> RemoveProductFromShop(int productId, int shopId)
    {
        var product = await _context.Products.FindAsync(productId);

        if (product is null) 
            return NotFound("The product does not exists");
        
        var shop = await _context.Shops.Include(s => s.Products).FirstOrDefaultAsync(s => s.Id == shopId);

        if (shop is null)
            return NotFound("The shop does not exists");
        
        if (!shop.Products.Contains(product))
            return BadRequest("The shop does not have that product");

        shop.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("RemoveProviderFromShop/provider/{providerId}/shop/{shopId}")]
    public async Task<IActionResult> RemoveProviderFromShop(int providerId, int shopId)
    {
        var provider = await _context.Providers.FindAsync(providerId);

        if (provider is null) 
            return NotFound("The provider does not exists");
        
        var shop = await _context.Shops.Include(s => s.Providers).FirstOrDefaultAsync(s => s.Id == shopId);

        if (shop is null)
            return NotFound("The shop does not exists");
        
        if (!shop.Providers.Contains(provider))
            return BadRequest("The shop does not have that provider");

        shop.Providers.Remove(provider);
        await _context.SaveChangesAsync();

        return Ok();
    }
}