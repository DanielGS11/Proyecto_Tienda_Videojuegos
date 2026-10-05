namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Order
{
    public int Id { get; set; }
    public float Price { get; set; }

    // Navigation to 'OrderProduct'
    public List<OrderProduct> OrderProducts { get; } = [];

    public DateTime CreatedAt { get; }
    public float TotalPrice { get; set; }
}