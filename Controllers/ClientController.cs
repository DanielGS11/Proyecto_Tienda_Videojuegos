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
    public async Task<IActionResult> GetClients(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");
        
        var result = await _context.Clients.Select(c => new
        {
            Id = c.Id,
            Name = c.Name,
            CityId = c.CityId
        }).Skip((page - 1) * 10).Take(10).ToListAsync();

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

    [HttpPut("UpdateClientCity/client/{clientId}/city/{cityId}")]
    public async Task<IActionResult> UpdateClientCity(int clientId, int cityId)
    {
        var client = await _context.Clients.FindAsync(clientId);

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