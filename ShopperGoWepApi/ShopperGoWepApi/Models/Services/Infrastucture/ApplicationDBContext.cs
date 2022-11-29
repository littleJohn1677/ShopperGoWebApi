// ===============================================================
// File name: ApplicationDBContext.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using Microsoft.EntityFrameworkCore;

using ShopperGoWepApi.Models.Entities;
using ShopperGoWepApi.Models.Enums;


namespace ShopperGoWepApi.Models.Services.Infrastucture
{
    public partial class ApplicationDBContext : DbContext
    {
        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<GeoLocation> GeoLocations { get; set; } = null!;
        public virtual DbSet<Country> Coutries { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!; 
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductsCategories { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configurazioni EXTRA
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<Currency>().HaveConversion<string>();
            // Convenzioni per SQLite
            configurationBuilder.Properties<decimal>().HaveConversion<float>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            FluentMappping(modelBuilder);
        }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
    }
}
