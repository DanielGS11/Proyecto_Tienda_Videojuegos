using Microsoft.AspNetCore.Mvc;
using Proyecto_Tienda_Videojuegos.Context;

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
        return Ok();
    }
}