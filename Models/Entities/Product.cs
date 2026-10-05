namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public abstract class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
}