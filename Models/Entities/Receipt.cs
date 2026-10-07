namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Receipt
{
    public int Id { get; set; }

    // FK de 'Order'
    public int OrderId { get; }

    public DateTime CreatedAt { get; set; }
    public float Price { get; set; }
}