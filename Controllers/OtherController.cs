using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.Entities;
using Proyecto_Tienda_Videojuegos.Models.Enums;

namespace Proyecto_Tienda_Videojuegos.Controllers;

[Route("api/other")]
[ApiController]
public class OtherController : ControllerBase
{
    private readonly VideogameDBContext _context;

    public OtherController(VideogameDBContext context)
    {
        _context = context;
    }

    [HttpGet("GetProducts")]
    public async Task<IActionResult> GetProducts(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");

        var result = await _context.Products.Select(p => new
        {
            Id = p.Id,
            Name = p.Name,
            Stock = p.Stock,
            Price = $"{p.Price}$",
            Type = p is Videogame ? "Videogame" : "Platform"
        }).Skip((page - 1) * 10).Take(10).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetProduct/{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var result = await _context.Products.Where(p => p.Id == id).Select(p => new
        {
            Id = p.Id,
            Name = p.Name,
            Stock = p.Stock,
            Price = $"{p.Price}$",
            Type = p is Videogame ? "Videogame" : "Platform"
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPut("UpdateProductStock/{id}")]
    public async Task<IActionResult> UpdateProductStock(int id, int stock)
    {
        if (stock < 1)
            return BadRequest("The stock cannot be below 1");

        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return NotFound();

        product.Stock = stock;
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("ResetDB")]
    public async Task<IActionResult> ResetDB()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.MigrateAsync();

        var city1 = new City { Name = "City 1" };
        var city2 = new City { Name = "City 2" };
        var city3 = new City { Name = "City 3" };

        _context.Cities.AddRange([city1, city2, city3]);
        await _context.SaveChangesAsync();

        var platform1 = new Platform { Name = "Platform 1", Stock = 4, Price = 329, Company = "Company 1" };
        var platform2 = new Platform { Name = "Platform 2", Stock = 6, Price = 650, Company = "Company 2" };
        var platform3 = new Platform { Name = "Platform 3", Stock = 9, Price = 699.99f, Company = "Company 3" };
        var platform4 = new Platform { Name = "Platform 4", Stock = 11, Price = 500, Company = "Company 4" };

        _context.Platforms.AddRange([platform1, platform2, platform3, platform4]);
        await _context.SaveChangesAsync();

        var videogame1 = new Videogame
        {
            Name = "Game 1",
            Stock = 60,
            Price = 44.99f,
            Developer = "Dev 1",
            Genres = [Genre.RPG, Genre.Adventure],
            Platforms = [platform1]
        };
        var videogame2 = new Videogame
        {
            Name = "Game 2",
            Stock = 10,
            Price = 59.99f,
            Developer = "Dev 2",
            Genres = [Genre.OpenWorld, Genre.Action],
            Platforms = [platform1, platform3, platform4]
        };
        var videogame3 = new Videogame
        {
            Name = "Game 3",
            Stock = 8,
            Price = 50,
            Developer = "Dev 3",
            Genres = [Genre.Action],
            Platforms = [platform2]
        };

        _context.Videogames.AddRange([videogame1, videogame2, videogame3]);
        await _context.SaveChangesAsync();

        var provider1 = new Provider
        {
            Name = "Provider 1",
            Products = [platform1, videogame1],
            Cities = [city1, city3]
        };
        var provider2 = new Provider
        {
            Name = "Provider 2",
            Products = [platform3, videogame2],
            Cities = [city3]
        };
        var provider3 = new Provider
        {
            Name = "Provider 3",
            Products = [platform1, platform2, platform3,
            platform4, videogame1, videogame2, videogame3],
            Cities = [city1, city2, city3]
        };

        _context.Providers.AddRange([provider1, provider2, provider3]);
        await _context.SaveChangesAsync();

        var shop1 = new Shop
        {
            Name = "Shop 1",
            Cities = [city1, city2, city3],
            Products = [platform1, platform2, platform3,
            videogame1, videogame2, videogame3],
            Providers = [provider1, provider3]
        };
        var shop2 = new Shop
        {
            Name = "Shop 2",
            Products = [platform4, videogame1, videogame2, videogame3],
            Providers = [provider1, provider3]
        };

        _context.Shops.AddRange([shop1, shop2]);
        await _context.SaveChangesAsync();

        var employee1 = new Employee { Name = "Employee 1", CityId = city1.Id, ShopId = shop1.Id };
        var employee2 = new Employee { Name = "Employee 2", CityId = city2.Id, ShopId = shop2.Id };
        var employee3 = new Employee { Name = "Employee 3", CityId = city3.Id, ShopId = shop1.Id };

        _context.Employees.AddRange([employee1, employee2, employee3]);
        await _context.SaveChangesAsync();

        var client1 = new Client { Name = "Client 1", CityId = city1.Id };
        var client2 = new Client { Name = "Client 2", CityId = city3.Id };
        var client3 = new Client { Name = "Client 3", CityId = city2.Id };

        _context.Clients.AddRange([client1, client2, client3]);
        await _context.SaveChangesAsync();

        var order1 = new Order { ClientId = client1.Id, CreatedAt = DateTime.Now };
        var order2 = new Order
        {
            ClientId = client2.Id,
            CreatedAt = DateTime.ParseExact("11/02/2000 17:05:20", 
            "dd/MM/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture)
        };

        _context.Orders.AddRange([order1, order2]);
        await _context.SaveChangesAsync();

        var orderProduct1 = new OrderProduct
        {
            OrderId = order1.Id,
            ProductId = platform1.Id,
            ProductName = platform1.Name,
            Quantity = 2
        }; orderProduct1.Price = orderProduct1.Quantity * platform1.Price;
        var orderProduct2 = new OrderProduct
        {
            OrderId = order1.Id,
            ProductId = platform4.Id,
            ProductName = platform4.Name,
            Quantity = 1
        }; orderProduct2.Price = orderProduct2.Quantity * platform4.Price;
        var orderProduct3 = new OrderProduct
        {
            OrderId = order1.Id,
            ProductId = videogame2.Id,
            ProductName = videogame2.Name,
            Quantity = 3
        }; orderProduct3.Price = orderProduct3.Quantity * videogame2.Price;
        var orderProduct4 = new OrderProduct
        {
            OrderId = order2.Id,
            ProductId = videogame3.Id,
            ProductName = videogame3.Name,
            Quantity = 1
        }; orderProduct4.Price = orderProduct4.Quantity * videogame3.Price;

        _context.OrderProducts.AddRange([orderProduct1, orderProduct2, orderProduct3, orderProduct4]);

        order1.TotalPrice = order1.OrderedProducts.Sum(o => o.Price);
        order2.TotalPrice = order2.OrderedProducts.Sum(o => o.Price);
        await _context.SaveChangesAsync();

        var receipt = new Receipt
        {
            OrderId = order2.Id,
            Price = order2.TotalPrice,
            CreatedAt = DateTime.ParseExact("17/04/2000 19:32:42",
            "dd/MM/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture)
        };

        _context.Receipts.AddRange([receipt]);
        await _context.SaveChangesAsync();

        return Ok();
    }
}