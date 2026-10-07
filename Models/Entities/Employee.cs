namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }

    // FK de 'City'
    public int CityId { get; }

    // FK de 'Shop'
    public int ShopId { get; }
}