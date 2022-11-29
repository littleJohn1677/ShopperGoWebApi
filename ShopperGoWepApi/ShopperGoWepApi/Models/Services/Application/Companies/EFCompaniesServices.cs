// ===============================================================
// File name: EFCompaniesServices.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using ShopperGoWepApi.Models.Entities;
using ShopperGoWepApi.Models.Services.Infrastucture;

namespace ShopperGoWepApi.Models.Services.Application.Companies
{
    /// <summary>
    /// La classe <c>EFCompaniesService</c> controlla l'accesso alla banca dati
    /// </summary>
    /// <remarks>
    /// L'accesso ai dati avviene con EntityframeworkCore 6.11
    /// </remarks>
    public class EFCompaniesService : Repository<Company>
    {
        public EFCompaniesService(ApplicationDBContext dbContext) : base(dbContext)
        {
        }
    }
}