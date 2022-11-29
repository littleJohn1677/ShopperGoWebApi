// ===============================================================
// File name: Address.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using System.Text.Json.Serialization;

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>Address</c> modella un indirizzo.
    /// </summary>
    /// <remarks>
    /// Gli indirizzi possono essere italiani o esteri
    /// </remarks>
    [Serializable]
    public class Address
    {
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Indirizzo completo
        /// </summary>
        /// <example>Via dei banchi vecchi n.1</example>
        public string Location { get; set; } = null!;
        /// <summary>
        /// Cap
        /// </summary>
        public string PostalCode { get; set; } = null!;
        /// <summary>
        /// Coordinate di geo-localizzazione
        /// </summary>
        public GeoLocation? GeoLocation { get; set; } = null!;

        /// <summary>
        /// Comune o città associata
        /// </summary>
        public City City { get; set; } = null!;


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
        /// Questo costruttore inizializza un nuovo indirizzo
        /// </summary>
        public Address()
        {

        }
        /// <summary>
        /// Questo costruttore inizializza un nuovo indirizzo
        /// (<paramref name="location"/>,<paramref name="comune"/>,<paramref name="pv"/>).
        /// </summary>
        /// <param name="location">Indirizzo completo</param>
        /// <param name="comune">Comune italiano</param>
        /// <param name="pv">Provincia</param>
        public Address(string location, string comune, string pv, string postalCode)
        {
            this.Location = location;
            this.PostalCode = postalCode;
            this.City = new City(comune, pv);
        }
        /// <summary>
        /// Questo costruttore inizializza un nuovo indirizzo
        /// (<paramref name="location"/>,<paramref name="citta"/>,<paramref name="siglaNazione"/>,<paramref name="nomeNazione"/>).
        /// </summary>
        /// <param name="location">Indirizzo completo</param>
        /// <param name="citta">Citta</param>
        /// <param name="siglaNazione">Sigla nazione</param>
        public Address(string location, string citta, string siglaNazione)
        {
            this.Location = location;
            this.City = new City(citta, "EE") 
            { 
                Country = new Country(siglaNazione) 
            };
        }
        /// <summary>
        /// Questo costruttore inizializza un nuovo indirizzo
        /// (<paramref name="location"/>,<paramref name="citta"/>,<paramref name="siglaNazione"/>,<paramref name="nomeNazione"/>).
        /// </summary>
        /// <param name="location">Indirizzo completo</param>
        /// <param name="citta">Citta</param>
        /// <param name="siglaNazione">Sigla nazione</param>
        public Address(string location, string citta, string siglaNazione, decimal latitude, decimal longitude)
        {
            this.Location = location;
            this.City = new City(citta, "EE")
            {
                Country = new Country(siglaNazione),
            };
            this.GeoLocation = new GeoLocation(latitude, longitude);
        }
    }
}
