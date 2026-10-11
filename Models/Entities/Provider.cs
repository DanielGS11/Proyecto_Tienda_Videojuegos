using Microsoft.EntityFrameworkCore;

namespace Proyecto_Tienda_Videojuegos.Models.Entities;

// Set Name as unique value to avoid duplicates
[Index(nameof(Name), IsUnique = true)]
public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation to the Products (Videogame and Platform)
    public List<Product> Products { get; set; } = [];

    // Navigation to the Cities
    public List<City> Cities { get; set; } = [];
}