// ===============================================================
// File name: GeoLocation.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using System.Text.Json.Serialization;

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>GeoLocation</c> modella le coordinate di geo-locatizzazione.
    /// </summary>
    [Serializable]
    public class GeoLocation
    {
        public int Id { get; set; }

        /// <summary>
        /// Latitudine
        /// </summary>
        /// <example>51.3758010864258</example>
        public decimal Latitude { get; set; }
        /// <summary>
        /// Longitudine
        /// </summary>
        /// <example>-2.35989999771118</example>
        public decimal Longitude { get; set; }
        public int AddressId { get; set; }
        [JsonIgnore]
        public Address? Address { get; set; }


        /// <summary>
        /// Questo costruttore inizializza le coordinate di geo-localizzazione
        /// (<paramref name="name"/>,<paramref name="country"/>).
        /// <example>
        /// Per esempio:
        /// <code>
        /// GeoLocation g = new GeoLocation(51.3758010864258,-2.35989999771118);
        /// </code>
        /// <param name="latitude">Latidudine</param>
        /// <param name="longitude">Longitudine</param>
        /// </example>
        /// </summary>
        public GeoLocation(decimal latitude, decimal longitude) 
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
