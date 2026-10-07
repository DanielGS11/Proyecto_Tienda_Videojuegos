namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navegación hacia una lista general de 'Product' ('Videogame' y 'Platform')
    public List<Product> Products { get; set; } = [];

    // Navegación hacia la lista de 'City'
    public List<City> Cities { get; set; } = [];
}