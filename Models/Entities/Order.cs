namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Order
{
    public int Id { get; set; }
    
    // FK de 'Client'
    public int ClientId { get; }

    // Navegación hacia una lista de 'OrderProduct'
    public List<OrderProduct> OrderProducts { get; } = [];

    public DateTime CreatedAt { get; }
    public float TotalPrice { get; set; }

    // Navegación opcional hacia 'Receipt'
    public Receipt? Receipt { get; set; } = null;
}