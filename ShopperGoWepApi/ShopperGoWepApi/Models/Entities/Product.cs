// ===============================================================
// File name: Product.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using ShopperGoWepApi.Models.ValuesObjects;
using System.Text.Json.Serialization;

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>Product</c> modella un prodotto.
    /// </summary>
    [Serializable]
    public class Product
    {
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Nome del prodotto
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Descrizione del prodotto
        /// </summary>
        public string? Description { get; set; } = null!;
        public int Quantity { get; set; }
        /// <summary>
        /// Prezzo del prodotto
        /// </summary>
        public Money Price { get; set; } = null!;
        /// <summary>
        /// Categoria del prodotto associta
        /// </summary>
        public ProductCategory? Category { get; set; } = null!;


        /// <summary>
        /// Identificativo della compagnia associata al prodotto
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// Compagnia associata al prodotto
        /// </summary>
        [JsonIgnore]
        public Company? Company { get; set; } = null!;


        /// <summary>
        /// Foto associate al prodotto
        /// </summary>
        public ICollection<ProductPhoto>? Photos { get; set; }


        /// <summary>
        /// Questo costruttore che inizializza un prodotto
        /// </summary>
        public Product()
        {

        }
        /// <summary>
        /// Questo costruttore inizializza un prodotto
        /// (<paramref name="name"/>).
        /// </summary>
        /// <param name="name">Nome del prodotto</param>
        /// <param name="quantity">Quantita di stock</param>
        /// <param name="price">Prezzo della categoria</param>
        /// <param name="category">Nome della categoria</param>
        public Product(string name, int quantity, Money price, string category)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            Category = new ProductCategory(category);
        }
        /// <summary>
        /// Questo costruttore inizializza un prodotto
        /// (<paramref name="name"/>).
        /// </summary>
        /// <param name="name">Nome del prodotto</param>
        /// <param name="quantity">Quantita di stock</param>
        /// <param name="price">Prezzo della categoria</param>
        /// <param name="category">Nome della categoria</param>
        public Product(string name, int quantity, Money price, ProductCategory category)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
            Category = category;
        }
    }
}