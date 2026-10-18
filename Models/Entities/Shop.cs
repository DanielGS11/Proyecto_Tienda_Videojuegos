using Microsoft.EntityFrameworkCore;

namespace Proyecto_Tienda_Videojuegos.Models.Entities;

// Set Name as unique value to avoid duplicates
[Index(nameof(Name), IsUnique = true)]
public class Shop
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation to the Cities
    public List<City> Cities { get; set; } = [];

    // Navigation to the Products (Videogame and Platform)
    public List<Product> Products { get; set; } = [];

    // Navigation to the Employees
    public List<Employee> Employees { get; set; } = [];

    // Navigation to the Providers
     public List<Provider> Providers { get; set; } = [];
    
}