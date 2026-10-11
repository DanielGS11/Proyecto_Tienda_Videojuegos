namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class OrderProduct
{
    public int Id { get; set; }
    
    // FK of the Order
    public int OrderId { get; set; }
    
    // Optional FK of the Product (Platform or Videogame)
    public int? ProductId { get; set; } = null;

    public string ProductName { get; set; }
    public int Quantity { get; set; } = 1;
    public float Price { get; set; }
}