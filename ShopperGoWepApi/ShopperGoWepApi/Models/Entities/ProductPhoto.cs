// ===============================================================
// File name: ProductPhoto.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using System.Text.Json.Serialization;

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>ProductPhoto</c> modella una foto associata ad un prodotto prodotto.
    /// </summary>
    //[Serializable]
    public class ProductPhoto
    {
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary>
        public string Guid { get; set; } = null!;


        /// <summary>
        /// Identificativo del prodotto associato alla foto
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Foto associata al prodototto
        /// </summary>
        [JsonIgnore]
        public Product? Product { get; set; } = null!;
    }
}
