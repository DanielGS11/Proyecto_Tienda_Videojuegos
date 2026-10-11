namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Order
{
    public int Id { get; set; }
    
    // FK of the Client
    public int ClientId { get; }

    // Navigation to the OrderProducts
    public List<OrderProduct> OrderProducts { get; } = [];

    public DateTime CreatedAt { get; }
    public float TotalPrice { get; set; }

    // Optional navigation to the Receipt
    public Receipt? Receipt { get; set; } = null;
}