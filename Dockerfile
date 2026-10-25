# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Proyecto_Tienda_Videojuegos.csproj", "./"]
RUN dotnet restore "Proyecto_Tienda_Videojuegos.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa de ejecución (producción)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Proyecto_Tienda_Videojuegos.dll"]