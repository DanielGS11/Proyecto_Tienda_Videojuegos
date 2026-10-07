using Proyecto_Tienda_Videojuegos.Models.Enums;

namespace Proyecto_Tienda_Videojuegos.Models.Entities;

public class Videogame : Product
{
    public string Developer {get; set; }

    public List<Genre> Genres { get; set; } = [];

    // Navegación hacia la lista de 'Platform'
    public List<Platform> Platforms { get; } = [];
}