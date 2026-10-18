using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.DTOs;
using Proyecto_Tienda_Videojuegos.Models.Entities;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/clients")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public ClientController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetClients")]
    public async Task<IActionResult> GetClients()
    {
        var result = await _context.Clients.Select(c => new
        {
            Id = c.Id,
            Name = c.Name,
            CityId = c.CityId
        }).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetClient/{id}")]
    public async Task<IActionResult> GetClient(int id)
    {
        var result = await _context.Clients.Where(c => c.Id == id).Select(c => new
        {
            Id = c.Id,
            Name = c.Name,
            CityId = c.CityId
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("CreateClient")]
    public async Task<IActionResult> CreateClient([FromBody] ClientDto clientDto)
    {
        var cityExists = await _context.Cities.AnyAsync(c => c.Id == clientDto.CityId);

        if (!cityExists)
            return NotFound();

        _context.Clients.Add(new Client { Name = clientDto.Name, CityId = clientDto.CityId });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("UpdateClient/{id}")]
    public async Task<IActionResult> UpdateClient(int id, string newName)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client is null)
            return NotFound();
        
        client.Name = newName;
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("UpdateClientCity/client/{id}/city/{cityId}")]
    public async Task<IActionResult> UpdateClientCity(int id, int cityId)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client is null)
            return NotFound("The client does not exists");

        var cityExists = await _context.Cities.AnyAsync(c => c.Id == cityId);

        if (!cityExists)
            return NotFound("The city does not exists");
        
        client.CityId = cityId;
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("DeleteClient/{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var rowsAffected = await _context.Clients.Where(c => c.Id == id).ExecuteDeleteAsync();

        if (rowsAffected <= 0)
            return NotFound();

        return Ok();
    }
}