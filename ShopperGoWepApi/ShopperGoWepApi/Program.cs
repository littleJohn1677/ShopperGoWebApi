// ===============================================================
// File name: Program.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShopperGoWepApi.Middleware;
using ShopperGoWepApi.Models.Entities;
using ShopperGoWepApi.Models.Services.Application.Companies;
using ShopperGoWepApi.Models.Services.Infrastucture;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ShopperGoWepApi
{
    public class Program
    {
        private static void SetSwaggerOptions(SwaggerGenOptions option)
        {
            option.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Description = "Inserire la chiave di autenticazione",
                Name = "ApiKey",
                In = ParameterLocation.Header,
                Scheme = "ApiKey"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    new string[] {}
                }
            });
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                SetSwaggerOptions(option);
            });

            // Database
            builder.Services.AddDbContextPool<ApplicationDBContext>(optionsBuilder =>
            {
                string connectionString = builder.Configuration["ConnectionStrings:Sqlite"];
                optionsBuilder.UseSqlite(connectionString, options =>
                {
                    // Disabilito i tentativi di connessione Sqlite NON supportati.
                    // options.EnableRetryOnFailure(3);
                });
            });

            builder.Services.AddTransient<IRepository<Company>, Repository<Company>>();
            builder.Services.AddTransient<EFCompaniesService>();
            builder.Services.AddTransient<IRepository<Product>, Repository<Product>>();
            builder.Services.AddTransient<EFProductsService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<TokenMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}