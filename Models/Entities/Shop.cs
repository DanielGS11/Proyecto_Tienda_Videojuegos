namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Shop
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navegación hacia la lista de 'City'
    public List<City> Cities { get; }

    // Navegación hacia una lista general de 'Product' ('Videogame' y 'Platform')
    public List<Product> Products { get; } = [];

    // Navegación hacia la lista de 'Employee'
    public List<Employee> Employees { get; } = [];

    // Navegación hacia la lista de 'Provider'
     public List<Provider> Providers { get; } = [];
    
}