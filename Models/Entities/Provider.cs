namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation to all the products ('Videogame' and 'Console')
    public List<Product> Products { get; set; } = [];

    public List<City> Destinations { get; set; } = [];
}