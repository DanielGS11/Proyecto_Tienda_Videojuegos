using Microsoft.EntityFrameworkCore;

namespace Proyecto_Tienda_Videojuegos.Models.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Shop
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation to the Cities
    public List<City> Cities { get; }

    // Navigation to the Products (Videogame and Platform)
    public List<Product> Products { get; } = [];

    // Navigation to the Employees
    public List<Employee> Employees { get; } = [];

    // Navigation to the Providers
     public List<Provider> Providers { get; } = [];
    
}