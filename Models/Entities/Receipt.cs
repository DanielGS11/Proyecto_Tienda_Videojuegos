namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Receipt
{
    public int Id { get; set; }

    // FK of the Order
    public int OrderId { get; set; }

    public DateTime CreatedAt { get; set; }
    public float Price { get; set; }
}