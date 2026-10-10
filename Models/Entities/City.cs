using Microsoft.EntityFrameworkCore;

namespace Proyecto_Tienda_Videojuegos.Models.Entities;

// Set City.Name as unique value to avoid duplicates
[Index(nameof(Name), IsUnique = true)]
public class City
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation to the Shops
    public List<Shop> Shops { get; } = [];
}