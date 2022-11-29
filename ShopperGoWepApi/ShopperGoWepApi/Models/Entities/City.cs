// ===============================================================
// File name: City.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using System.Text.Json.Serialization;

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>City</c> modella un comune italiano o una citta estera.
    /// </summary>
    [Serializable]
    public class City
    {
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Nome del comune/città
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Provincia della comune
        /// </summary>
        public string PV { get; set; } = null!;

        /// <summary>
        /// Nazione associata al comune/città
        /// </summary>
        [JsonIgnore]
        public Country? Country { get; set; }

        /// <summary>
        /// Elenco degli indirizzi associati
        /// </summary>
        [JsonIgnore]
        public ICollection<Address>? Addresses { get; set; }

        /// <summary>
        /// Questo costruttore inizializza un nuovo comune italiano
        /// </summary>
        public City()
        {

        }
        /// <summary>
        /// Questo costruttore inizializza un nuovo comune italiano
        /// (<paramref name="name"/>,<paramref name="pv"/>).
        /// </summary>
        /// <param name="name">Comune italiano</param>
        /// <param name="pv">Provincia</param>
        public City(string name, string pv) 
        { 
            this.Name = name;
            this.PV = pv;
            this.Country = new Country("IT", "ITALIA");
        }
        /// <summary>
        /// Questo costruttore inizializza una nuova città estera
        /// (<paramref name="name"/>,<paramref name="country"/>).
        /// </summary>
        /// <param name="name">Nome della città</param>
        /// <param name="country">Nazione della città</param>
        public City(string name, Country country)
        {
            this.Name = name;
            this.PV = "EE";
            this.Country = country; 
        }
    }
}
