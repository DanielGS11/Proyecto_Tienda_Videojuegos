using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.Entities;
using Proyecto_Tienda_Videojuegos.Models.DTOs;

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
    public async Task<IActionResult> GetPlatforms(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");

        var result = await _context.Platforms.Select(p => new
        {
            Id = p.Id,
            Name = p.Name,
            Stock = p.Stock,
            Company = p.Company,
            Price = $"{p.Price}$"
        }).Skip((page - 1) * 10).Take(10).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetPlatform/{id}")]
    public async Task<IActionResult> GetPlatform(int id)
    {
        var result = await _context.Platforms.Where(p => p.Id == id).Select(p => new
        {
            Id = p.Id,
            Name = p.Name,
            Stock = p.Stock,
            Company = p.Company,
            Price = $"{p.Price}$"
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("CreatePlatform")]
    public async Task<IActionResult> CreatePlatform([FromBody] ProductDto platformDto)
    {
        var nameExists = await _context.Platforms.AnyAsync(p =>
            p.Name.ToLower().Trim() == platformDto.Name.ToLower().Trim());

        if (nameExists)
            return BadRequest("There is already a platform with that name");

        _context.Platforms.Add(new Platform
        {
            Name = platformDto.Name,
            Price = platformDto.Price,
            Company = platformDto.Creator
        });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("UpdatePlatform/{id}")]
    public async Task<IActionResult> UpdatePlatform(int id, [FromBody] ProductDto platformUpdate)
    {
        var nameExists = await _context.Platforms.AnyAsync(p =>
            p.Name.ToLower().Trim() == platformUpdate.Name.ToLower().Trim());

        if (nameExists)
            return BadRequest("There is already a platform with that name");

        var rowsAffected = await _context.Platforms.Where(p => p.Id == id).ExecuteUpdateAsync(p =>
            p.SetProperty(p => p.Name, platformUpdate.Name)
            .SetProperty(p => p.Price, platformUpdate.Price)
            .SetProperty(p => p.Company, platformUpdate.Creator)
            );

        if (rowsAffected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("DeletePlatform/{id}")]
    public async Task<IActionResult> DeletePlatform(int id)
    {
        var rowsAffected = await _context.Platforms.Where(p => p.Id == id).ExecuteDeleteAsync();

        if (rowsAffected <= 0)
            return NotFound();

        return Ok();
    }
}