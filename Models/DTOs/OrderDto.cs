namespace Proyecto_Tienda_Videojuegos.Models.DTOs;

public class OrderDto
{
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}