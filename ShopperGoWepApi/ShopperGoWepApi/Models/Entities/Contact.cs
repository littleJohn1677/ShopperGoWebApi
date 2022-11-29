// ===============================================================
// File name: Contact.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using ShopperGoWepApi.Models.Enums;
using System.Text.Json.Serialization;

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>Contact</c> modella un contatto.
    /// </summary>
    [Serializable]
    public class Contact
    {
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Tipo di contatto
        /// </summary>
        public ContactType Type { get; set; }
        /// <summary>
        /// Valore del contatto riferito al tipo
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Ordine di visualizzazione/importanza
        /// </summary>
        public int Order { get; set; } = 100;


        /// <summary>
        /// Identificativo della compagnia associata al contatto
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// Compagnia associata al contatto
        /// </summary>
        [JsonIgnore]
        public Company? Company { get; set; }

        public Contact()
        {

        }

        /// <summary>
        /// Questo costruttore inizializza un nuovo contatto
        /// (<paramref name="email"/>).
        /// </summary>
        /// <param name="email">Indirizzo di posta elettronica</param>
        public Contact(string email)
        {
            Type = ContactType.Email;
            Value = email;
            Order = 1;
        }
        /// <summary>
        /// Questo costruttore inizializza un nuovo contatto
        /// (<paramref name="type"/>,<paramref name="value"/>,<paramref name="order"/>).
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="order"></param>
        public Contact(ContactType type, string value, int order)
        {
            Type = type;
            Value = value;
            Order = order;
        }
    }
}
