namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class OrderProduct
{
    public int Id { get; set; }
    // Navigation to 'Order'
    public int OrderId { get; set; }

    // Navigation to the Product ('Videogame' or 'Console')
    public Product Product { get; set; }

    public int? ConsoleId { get; set; } = null;
    public int? VideogameId { get; set; } = null;
    public int Quantity { get; set; } = 1;
    public float Price { get; set; }
}