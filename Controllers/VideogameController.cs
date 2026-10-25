using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.DTOs;
using Proyecto_Tienda_Videojuegos.Models.Entities;
using Proyecto_Tienda_Videojuegos.Models.Enums;

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
    public async Task<IActionResult> GetVideogames(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");

        var result = await _context.Videogames.Select(v => new
        {
            Id = v.Id,
            Name = v.Name,
            Stock = v.Stock,
            Developer = v.Developer,
            Genres = v.Genres,
            Price = $"{v.Price:C}",
            Platforms = v.Platforms.Select(p => p.Name).ToList()
        }).Skip((page - 1) * 10).Take(10).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetVideogame/{id}")]
    public async Task<IActionResult> GetVideogame(int id)
    {
        var result = await _context.Videogames.Where(v => v.Id == id).Select(v => new
        {
            Id = v.Id,
            Name = v.Name,
            Stock = v.Stock,
            Developer = v.Developer,
            Genres = v.Genres,
            Price = $"{v.Price:C}",
            Platforms = v.Platforms.Select(p => p.Name).ToList()
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("CreateVideogame")]
    public async Task<IActionResult> CreateVideogame([FromBody] ProductDto videogameDto)
    {
        var nameExists = await _context.Videogames.AnyAsync(p =>
            p.Name.ToLower().Trim() == videogameDto.Name.ToLower().Trim());

        if (nameExists)
            return BadRequest("There is already a videogame with that name");

        _context.Videogames.Add(new Videogame
        {
            Name = videogameDto.Name,
            Price = videogameDto.Price,
            Developer = videogameDto.Creator,
        });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("AddPlatformToGame/platform/{platformId}/videogame/{gameId}")]
    public async Task<IActionResult> AddPlatformToGame(int platformId, int gameId)
    {
        var platform = await _context.Platforms.FindAsync(platformId);

        if (platform is null)
            return NotFound("The platform does not exists");

        var videogame = await _context.Videogames.Include(v => v.Platforms).FirstOrDefaultAsync(v => v.Id == gameId);

        if (videogame is null)
            return NotFound("The videogame does not exists");
        
        if (videogame.Platforms.Contains(platform))
            return BadRequest("The videogame is already available on that platform");
        
        videogame.Platforms.Add(platform);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("AddGenreToGame/videogame/{gameId}")]
    public async Task<IActionResult> AddPlatformToGame(int gameId, Genre genre)
    {
        var videogame = await _context.Videogames.FindAsync(gameId);

        if (videogame is null)
            return NotFound("The videogame does not exists");
        
        if (videogame.Genres.Contains(genre))
            return BadRequest("The videogame is already from that genre");
        
        videogame.Genres.Add(genre);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("UpdateVideogame/{id}")]
    public async Task<IActionResult> UpdateVideogame(int id, [FromBody] ProductDto videogameUpdate)
    {
        var nameExists = await _context.Videogames.AnyAsync(p =>
            p.Name.ToLower().Trim() == videogameUpdate.Name.ToLower().Trim());

        if (nameExists)
            return BadRequest("There is already a videogame with that name");

        var rowsAffected = await _context.Videogames.Where(v => v.Id == id).ExecuteUpdateAsync(v =>
            v.SetProperty(v => v.Name, videogameUpdate.Name)
            .SetProperty(v => v.Price, videogameUpdate.Price)
            .SetProperty(v => v.Developer, videogameUpdate.Creator)
            );

        if (rowsAffected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("DeleteVideogame/{id}")]
    public async Task<IActionResult> DeleteVideogame(int id)
    {
        var rowsAffected = await _context.Videogames.Where(v => v.Id == id).ExecuteDeleteAsync();

        if (rowsAffected <= 0)
            return NotFound();

        return Ok();
    }

    [HttpDelete("RemovePlatformFromGame/platform/{platformId}/videogame/{gameId}")]
    public async Task<IActionResult> RemovePlatformFromGame(int platformId, int gameId)
    {
        var platform = await _context.Platforms.FindAsync(platformId);

        if (platform is null)
            return NotFound("The platform does not exists");

        var videogame = await _context.Videogames.Include(v => v.Platforms).FirstOrDefaultAsync(v => v.Id == gameId);

        if (videogame is null)
            return NotFound("The videogame does not exists");
        
        if (!videogame.Platforms.Contains(platform))
            return BadRequest("The videogame is not available on that platform");
        
        videogame.Platforms.Remove(platform);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("RemoveGenreFromGame/videogame/{gameId}")]
    public async Task<IActionResult> RemoveGenreFromGame(int gameId, Genre genre)
    {
        var videogame = await _context.Videogames.FindAsync(gameId);

        if (videogame is null)
            return NotFound("The videogame does not exists");
        
        if (!videogame.Genres.Contains(genre))
            return BadRequest("The videogame is not from that genre");
        
        videogame.Genres.Remove(genre);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
