// ===============================================================
// File name: Country.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>Country</c> modella una nazione.
    /// </summary>
    // [Serializable]
    public class Country
    {
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Codice sigla nazione
        /// </summary>
        /// <example>IT = Italia</example>
        public string Code { get; set; }
        /// <summary>
        /// Nome della nazione
        /// </summary>
        /// <example>ITALIA</example>
        public string? Name { get; set; } = null!;

        /// <summary>
        /// Elenco delle citta associate alla nazione
        /// </summary>
        public ICollection<City> Cities { get; set; } = null!;

        /// <summary>
        /// Questo costruttore che inizializza la nazione
        /// (<paramref name="code"/>).
        /// <example>
        /// Per esempio:
        /// <code>
        /// Country code = new Country("IT");
        /// </code>
        /// <param name="code">Sigla della nazione</param>
        /// </example>
        /// </summary>
        public Country(string code)
        {
            Code = code;
        }
        /// <summary>
        /// Questo costruttore che inizializza la nazione
        /// (<paramref name="code"/>,<paramref name="name"/>).
        /// <example>
        /// Per esempio:
        /// <code>
        /// Country country = new Country("IT","ITALIA");
        /// </code>
        /// <param name="code">Sigla della nazione</param>
        /// <param name="name">Nome della nazione</param>
        /// </example>
        /// </summary>
        public Country(string code, string name)
        {
            Code = code;
            Name = name;   
        }
    }
}