namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Shop
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation to 'City'
    public City City { get; set; }

    // Navigation to all the products ('Videogame' and 'Console')
    public List<Product> Products { get; } = [];

    // Navigation to 'Employee'
    public List<Employee> Employees { get; } = [];

    // Navigation to 'Providers'
     public List<Provider> Providers { get; } = [];
    
}