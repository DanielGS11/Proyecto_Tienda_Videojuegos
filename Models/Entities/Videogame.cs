using Proyecto_Tienda_Videojuegos.Models.Enums;

namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Videogame : Product
{
    public List<Genre> Genres { get; set; } = [];

    // Navigation to 'Console'
    public List<Console> Platforms { get; } = [];
}