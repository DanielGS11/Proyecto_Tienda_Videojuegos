using Microsoft.EntityFrameworkCore;

namespace Proyecto_Tienda_Videojuegos.Models.Entities;

// Set Name as unique value to avoid duplicates
[Index(nameof(Name), IsUnique = true)]
public abstract class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
}