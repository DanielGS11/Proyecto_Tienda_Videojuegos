using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Proyecto_Tienda_Videojuegos.Filter;

public class ControllerFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument docs, DocumentFilterContext context)
    {
        docs.Tags.Add(new OpenApiTag { Name = "Videogame" });
        docs.Tags.Add(new OpenApiTag { Name = "Platform" });
        docs.Tags.Add(new OpenApiTag { Name = "Shop" });
        docs.Tags.Add(new OpenApiTag { Name = "Employee" });
        docs.Tags.Add(new OpenApiTag { Name = "Client" });
        docs.Tags.Add(new OpenApiTag { Name = "Provider" });
        docs.Tags.Add(new OpenApiTag { Name = "City" });
        docs.Tags.Add(new OpenApiTag { Name = "Order" });
        docs.Tags.Add(new OpenApiTag { Name = "Other" });
    }
}