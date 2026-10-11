namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }

    // FK of the City
    public int CityId { get; set; }

    // FK of the Shop
    public int ShopId { get; set; }
}