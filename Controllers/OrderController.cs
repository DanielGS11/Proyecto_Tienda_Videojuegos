using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Context;
using Proyecto_Tienda_Videojuegos.Models.DTOs;
using Proyecto_Tienda_Videojuegos.Models.Entities;

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
    public async Task<IActionResult> GetOrders(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");

        var result = await _context.Orders.Select(o => new
        {
            Id = o.Id,
            ClientId = o.ClientId,
            OrderedProducts = o.OrderedProducts.Select(op => new
            {
                ProductName = op.ProductName,
                Quantity = op.Quantity,
                Price = op.Price
            }).ToList(),
            CreatedAt = o.CreatedAt,
            TotalPrice = $"{o.TotalPrice:C}",
            Receipt = o.Receipt == null ? null : new
            {
                Id = o.Receipt.Id,
                CreatedAt = o.Receipt.CreatedAt
            },
        }).Skip((page - 1) * 10).Take(10).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetOrder/{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var result = await _context.Orders.Where(o => o.Id == id).Select(o => new
        {
            Id = o.Id,
            ClientId = o.ClientId,
            OrderedProducts = o.OrderedProducts.Select(op => new
            {
                ProductName = op.ProductName,
                Quantity = op.Quantity,
                Price = op.Price
            }).ToList(),
            CreatedAt = o.CreatedAt,
            TotalPrice = $"{o.TotalPrice:C}",
            Receipt = o.Receipt == null ? null : new
            {
                Id = o.Receipt.Id,
                CreatedAt = o.Receipt.CreatedAt
            },
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("GetReceipts")]
    public async Task<IActionResult> GetReceipts(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");

        var result = await _context.Receipts.Select(r => new
        {
            Id = r.Id,
            OrderId = r.OrderId,
            CreatedAt = r.CreatedAt,
            Price = r.Price
        }).Skip((page - 1) * 10).Take(10).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetReceipt/{id}")]
    public async Task<IActionResult> GetReceipt(int id)
    {
        var result = await _context.Receipts.Where(r => r.Id == id).Select(r => new
        {
            Id = r.Id,
            OrderId = r.OrderId,
            CreatedAt = r.CreatedAt,
            Price = r.Price
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("GetOrderProducts")]
    public async Task<IActionResult> GetOrderProducts(int page = 1)
    {
        if (page < 1)
            return BadRequest("The page number must be greater than 0");

        var result = await _context.OrderProducts.Select(o => new
        {
            Id = o.Id,
            OrderId = o.OrderId,
            ProductId = o.ProductId,
            ProductName = o.ProductName,
            Quantity = o.Quantity,
            Price = o.Price
        }).Skip((page - 1) * 10).Take(10).ToListAsync();

        return Ok(result);
    }

    [HttpGet("GetOrderProduct/{id}")]
    public async Task<IActionResult> GetOrderProduct(int id)
    {
        var result = await _context.OrderProducts.Where(o => o.Id == id).Select(o => new
        {
            Id = o.Id,
            OrderId = o.OrderId,
            ProductId = o.ProductId,
            ProductName = o.ProductName,
            Quantity = o.Quantity,
            Price = o.Price
        }).FirstOrDefaultAsync();

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto newOrder)
    {
        if (newOrder.Quantity <= 0)
            return BadRequest("The quantity cannot be below 1");

        var clientExists = await _context.Clients.AnyAsync(c => c.Id == newOrder.ClientId);

        if (!clientExists)
            return NotFound("The client does not exists");

        var product = await _context.Products.FindAsync(newOrder.ProductId);

        if (product is null)
            return NotFound("The product does not exists");
        
        if (product.Stock < newOrder.Quantity)
            return BadRequest("The product has not enough stock for the order");

        var order = new Order
        {
            ClientId = newOrder.ClientId,
            OrderedProducts = [new OrderProduct{
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = newOrder.Quantity,
                Price = product.Price * newOrder.Quantity
            }],
            CreatedAt = DateTime.Now
        };

        order.TotalPrice = order.OrderedProducts.Sum(o => o.Price);
        product.Stock -= newOrder.Quantity;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("AddProductToOrder/product/{productId}/order/{orderId}")]
    public async Task<IActionResult> AddProductToOrder(int productId, int orderId, int quantity)
    {
        var order = await _context.Orders.Include(o => o.OrderedProducts).Include(o => o.Receipt).FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            return NotFound("The order does not exists");

        if (order.Receipt != null)
            return BadRequest("The order has a receipt generated and cannot be modified");

        var product = await _context.Products.FindAsync(productId);

        if (product is null)
            return NotFound("The product does not exists");
        
        if (product.Stock < quantity)
            return BadRequest("The product has not enough stock for the order");

        OrderProduct? orderProduct = order.OrderedProducts.Find(o => o.ProductId == productId);

        if (orderProduct is null)
        {
            orderProduct = new OrderProduct { OrderId = orderId, ProductId = productId, ProductName = product.Name };
            _context.OrderProducts.Add(orderProduct);
        }

        orderProduct.Quantity += quantity;

        orderProduct.Price = product.Price * orderProduct.Quantity;

        order.TotalPrice += (product.Price * quantity);
        product.Stock -= quantity;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("CreateReceipt/order/{orderId}")]
    public async Task<IActionResult> CreateReceipt(int orderId)
    {
        var order = await _context.Orders.Include(o => o.Receipt).FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            return NotFound("The order does not exists");

        if (order.Receipt != null)
            return BadRequest("The order has already a receipt");

        _context.Receipts.Add(new Receipt { OrderId = orderId, CreatedAt = DateTime.Now, Price = order.TotalPrice });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("DeleteOrder/{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.Include(o => o.OrderedProducts).FirstOrDefaultAsync(o => o.Id == id);

        if (order is null)
            return NotFound();
        
        var productIds = order.OrderedProducts.Where(o => o.ProductId != null).Select(o => o.ProductId).ToList();
        var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

        foreach (OrderProduct orderProduct in order.OrderedProducts)
        {
            if (orderProduct.ProductId != null)
            {
                var product = products.Find(p => p.Id == orderProduct.ProductId);
                product.Stock += orderProduct.Quantity;
            }
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("DeleteOrderProduct/{id}")]
    public async Task<IActionResult> DeleteOrderProduct(int id)
    {
        var orderProduct = await _context.OrderProducts.FindAsync(id);

        if (orderProduct is null)
            return NotFound();

        var order = await _context.Orders.Include(o => o.OrderedProducts).Include(o => o.Receipt).FirstOrDefaultAsync(o => o.Id == orderProduct.OrderId);

        if (order.Receipt != null)
            return BadRequest("The order has a receipt generated and cannot be modified");
        
        if (orderProduct.ProductId != null)
        {
            var product = await _context.Products.FindAsync(orderProduct.ProductId);
            product.Stock += orderProduct.Quantity;
        }

        order.TotalPrice -= orderProduct.Price;
        _context.OrderProducts.Remove(orderProduct);
        await _context.SaveChangesAsync();

        if (order.OrderedProducts.Count <= 0)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    [HttpDelete("DeleteReceipt/{id}")]
    public async Task<IActionResult> DeleteReceipt(int id)
    {
        var rowsAffected = await _context.Receipts.Where(r => r.Id == id).ExecuteDeleteAsync();

        if (rowsAffected <= 0)
            return NotFound();

        return Ok();
    }
}