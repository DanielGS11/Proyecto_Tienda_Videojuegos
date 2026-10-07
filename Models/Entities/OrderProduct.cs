namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class OrderProduct
{
    public int Id { get; set; }
    
    // FK de 'Order'
    public int OrderId { get; set; }
    
    // FK opcional de 'Product' (Platform o Videogame)
    public int? ProductId { get; set; } = null;

    public string ProductName { get; set; }
    public int Quantity { get; set; } = 1;
    public float Price { get; set; }
}