namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Order
{
    public int Id { get; set; }
    
    // FK of the Client
    public int ClientId { get; set; }

    // Navigation to the OrderProducts
    public List<OrderProduct> OrderedProducts { get; set; } = [];

    public DateTime CreatedAt { get; set; }
    public float TotalPrice { get; set; }

    // Optional navigation to the Receipt
    public Receipt? Receipt { get; set; } = null;
}