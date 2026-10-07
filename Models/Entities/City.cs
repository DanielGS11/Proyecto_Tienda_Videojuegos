namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navegación hacia una lista de 'Shop'
    public List<Shop> Shops { get; } = [];
}