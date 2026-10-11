namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }

    // FK of the City
    public int CityId { get; }
}