using Microsoft.AspNetCore.Mvc;
using Proyecto_Tienda_Videojuegos.Context;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public EmployeeController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetEmployees")]
    public async Task<IActionResult> GetEmployees()
    {
        return Ok();
    }
}