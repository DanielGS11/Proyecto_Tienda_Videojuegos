namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation to 'City'
    public City City { get; }

    // Navigation to 'Shop'
    public Shop Shop { get; }
}