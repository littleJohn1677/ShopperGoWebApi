// ===============================================================
// File name: ApplicationDBContextMapping.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using Microsoft.EntityFrameworkCore;
using ShopperGoWepApi.Models.Entities;
using ShopperGoWepApi.Models.ValuesObjects;

namespace ShopperGoWepApi.Models.Services.Infrastucture
{
    public partial class ApplicationDBContext
    {
        private static void FluentMappping(ModelBuilder modelBuilder)
        {
            // Mappatura per il proprio tipo
            modelBuilder.Owned<Money>();

            // Mappatura compagnia
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique(); // N.B. in fase di inserimento rimuovere tutta la punteggiatura e gli spazi
                entity.Property(e => e.Name).HasMaxLength(255);

                // Relazioni
                entity.HasMany(e => e.Contacts)
                      .WithOne(contact => contact.Company)
                      .HasForeignKey(e => e.CompanyId);

                entity.HasMany(e => e.Products)
                     .WithOne(product => product.Company)
                     .HasForeignKey(e => e.CompanyId);

                entity.HasMany(e => e.Addresses)
                    .WithOne(address => address.Company)
                    .HasForeignKey(e => e.CompanyId);
            });

            modelBuilder.Entity<Company>()
                .Navigation(e => e.Addresses)
                .AutoInclude();

            modelBuilder.Entity<Company>()
                .Navigation(e => e.Contacts)
                .AutoInclude();

            // Mappatura contatto
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.CompanyId, e.Type, e.Value }).IsUnique(); // Per una compagnia non ci devono essere contatti duplicati
            });

            // Mappatura address
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasIndex(entity => entity.Location);
                entity.Property(e => e.Location).HasMaxLength(255);

                // Relazioni
                entity.HasOne(e => e.City)
                    .WithMany(city => city.Addresses);

                // Relazioni
                entity.HasOne(e => e.GeoLocation)
                    .WithOne(geo => geo.Address);
            });

            modelBuilder.Entity<Address>()
                .Navigation(e => e.City)
                .AutoInclude();

            // Mappatura city
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Country)
                    .WithMany(country => country.Cities);

                entity.Property(entity => entity.Name).HasMaxLength(255);
                entity.Property(entity => entity.PV).HasMaxLength(2);
            });

            modelBuilder.Entity<City>()
                .Navigation(e => e.Country)
                .AutoInclude();

            // Mappatura country 
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();

                entity.Property(entity => entity.Code).HasMaxLength(3);
                entity.Property(entity => entity.Name).HasMaxLength(255);
            });

            // Mappatura product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(entity => entity.Id);
                entity.HasIndex(entity => entity.Name);
                entity.HasIndex(entity => entity.Description); // TODO: Spostare in campi metadato o liste, per cercare solo nelle chiavi.

                entity.Property(entity => entity.Name).HasMaxLength(50);
                entity.Property(entity => entity.Description).HasMaxLength(1024);

                //entity.HasOne(e => e.Company)
                //   .WithMany(company => company.Products)
                //   .HasForeignKey(e => e.Id); // TODO: Verificare se lo stesso prodotto può ricadere in categorie diverse.

                entity.HasOne(e => e.Category)
                    .WithMany(category => category.Products)
                    .HasForeignKey(e => e.Id); // TODO: Verificare se lo stesso prodotto può ricadere in categorie diverse.
            });

            modelBuilder.Entity<Product>()
                .Navigation(e => e.Category)
                .AutoInclude();

            modelBuilder.Entity<Product>()
                .Navigation(e => e.Photos)
                .AutoInclude(); 
        }
    }
}
