using Microsoft.EntityFrameworkCore;
using Proyecto_Tienda_Videojuegos.Models.Entities;

namespace Proyecto_Tienda_Videojuegos.Context;

public class VideogameDBContext : DbContext
{
    public VideogameDBContext(DbContextOptions options) : base(options) { }

    // DB Tables
    public DbSet<Videogame> Videogames { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // I used TPC so I can relate 'Platform' and 'Videogame', which are 'Product' subclasses
        modelBuilder.Entity<Product>().UseTpcMappingStrategy();

        // Videogame - Platform     N:M
        modelBuilder.Entity<Videogame>()
            .HasMany(v => v.Platforms)
            .WithMany()
            .UsingEntity("VideogamePlatforms");

        // Provider - Product (Videogame & Platform)        N:M
        modelBuilder.Entity<Provider>()
            .HasMany(p => p.Products)
            .WithMany()
            .UsingEntity("ProviderProducts");
        
        // Provider - City                                  N:M
        modelBuilder.Entity<Provider>()
            .HasMany(p => p.Cities)
            .WithMany()
            .UsingEntity("ProviderCities");
        
        // City - Shop                                      N:M
        modelBuilder.Entity<City>()
            .HasMany(c => c.Shops)
            .WithMany(s => s.Cities)
            .UsingEntity("CityShops");

        // Shop - Provider                                  N:M
        modelBuilder.Entity<Shop>()
            .HasMany(s => s.Providers)
            .WithMany()
            .UsingEntity("ShopProviders");
        
        // Shop - Product (Videogame & Platform)            N:M
        modelBuilder.Entity<Shop>()
            .HasMany(s => s.Products)
            .WithMany()
            .UsingEntity("ShopProducts");
        
        // Shop - Employee                                  N:1
        modelBuilder.Entity<Shop>()
            .HasMany(s => s.Employees)
            .WithOne()
            .HasForeignKey(e => e.ShopId)
            .IsRequired();
        
        // Employee - City                                  1:N
        modelBuilder.Entity<Employee>()
            .HasOne<City>()
            .WithMany()
            .HasForeignKey(e => e.CityId)
            .IsRequired();
        
        // Client - City                                    1:N
        modelBuilder.Entity<Client>()
            .HasOne<City>()
            .WithMany()
            .HasForeignKey(c => c.CityId)
            .IsRequired();
        
        // Order - Client                                     1:N
        modelBuilder.Entity<Order>()
            .HasOne<Client>()
            .WithMany()
            .HasForeignKey(o => o.ClientId)
            .IsRequired();
        
        // Order - Receipt                                     1:1
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Receipt)
            .WithOne()
            .HasForeignKey<Receipt>(r => r.OrderId)
            .IsRequired();
        
        // Order - OrderProduct                             N:1
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderedProducts)
            .WithOne()
            .HasForeignKey(op => op.OrderId)
            .IsRequired();
        
        // OrderProduct - Product (Videogame & Platform)    1:N
        modelBuilder.Entity<OrderProduct>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}