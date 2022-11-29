// ===============================================================
// File name: ProductCategory.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using System.Text.Json.Serialization;

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>ProductCategory</c> modella una categoria di prodotto.
    /// </summary>
    // [Serializable]
    public class ProductCategory
    {
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Nome della categoria
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Descrizione della categoria
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Prodotti associati alla categoria
        /// </summary>
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }

        /// <summary>
        /// Questo costruttore inizializza una nuova categoria di prodotto.
        /// </summary>
        public ProductCategory()
        {

        }

        /// <summary>
        /// Questo costruttore inizializza una nuova categoria di prodotto
        /// (<paramref name="name"/>).
        /// </summary>
        /// <param name="name">Nome della categoria</param>
        public ProductCategory(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Questo costruttore inizializza una nuova categoria di prodotto
        /// (<paramref name="name"/>, <paramref name="description"/>).
        /// </summary>
        /// <param name="name">Nome della categoria</param>
        /// <param name="description">descrizione</param>
        public ProductCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
