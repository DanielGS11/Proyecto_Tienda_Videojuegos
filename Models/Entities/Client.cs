namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation to 'City'
    public City City { get; }
}