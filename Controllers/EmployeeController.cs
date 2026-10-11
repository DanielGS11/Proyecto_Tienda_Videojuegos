using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.Entities;

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
        var result = await _context.Employees.Select(e => new
        {
            Id = e.Id,
            Name = e.Name,
            CityId = e.CityId,
            ShopId = e.ShopId
        }).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetEmployee/{id}")]
    public async Task<IActionResult> GetEmployee(int id)
    {
        var result = await _context.Employees.Where(e => e.Id == id).Select(e => new
        {
            Id = e.Id,
            Name = e.Name,
            CityId = e.CityId,
            ShopId = e.ShopId
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("CreateEmployee")]
    public async Task<IActionResult> CreateEmployee(string name, int cityId, int shopId)
    {
        var cityExists = await _context.Cities.AnyAsync(c => c.Id == cityId);

        if (!cityExists)
            return NotFound("The city does not exist");

        var shopExists = await _context.Shops.AnyAsync(s => s.Id == shopId);

        if (!shopExists)
            return NotFound("The shop does not exist");

        _context.Employees.Add(new Employee { Name = name, CityId = cityId, ShopId = shopId});
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("UpdateEmployee/{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, string newName)
    {
        var rowsAffected = await _context.Employees.Where(e => e.Id == id).ExecuteUpdateAsync(e =>
            e.SetProperty(e => e.Name, newName));
        
        if (rowsAffected == 0)
            return NotFound();

        return Ok();
    }

    [HttpPut("UpdateEmployeeCity/employee/{employeeId}/city/{cityId}")]
    public async Task<IActionResult> UpdateEmployeeCity(int employeeId, int cityId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        
        if (employee is null)
            return NotFound("The employee does not exists");
        
        var city = await _context.Cities.FindAsync(cityId);

        if (city is null)
            return NotFound("The city does not exists");
        
        employee.CityId = cityId;
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut("UpdateEmployeeShop/employee/{employeeId}/shop/{shopId}")]
    public async Task<IActionResult> UpdateEmployeeShop(int employeeId, int shopId)
    {
        var employee = await _context.Employees.FindAsync(employeeId);
        
        if (employee is null)
            return NotFound("The employee does not exists");
        
        var shop = await _context.Shops.FindAsync(shopId);

        if (shop is null)
            return NotFound("The shop does not exists");
        
        employee.ShopId = shopId;
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("DeleteEmployee/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var rowsAffected = await _context.Employees.Where(e => e.Id == id).ExecuteDeleteAsync();

        if (rowsAffected <= 0) 
            return NotFound();

        return Ok();
    }
}