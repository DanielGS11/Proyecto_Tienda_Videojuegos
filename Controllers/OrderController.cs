using Microsoft.AspNetCore.Mvc;
using Proyecto_Tienda_Videojuegos.Context;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public OrderController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetOrders")]
    public async Task<IActionResult> GetOrders()
    {
        return Ok();
    }
}