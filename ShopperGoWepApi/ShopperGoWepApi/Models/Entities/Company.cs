// ===============================================================
// File name: Company.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

namespace ShopperGoWepApi.Models.Entities
{
    /// <summary>
    /// La classe <c>Company</c> modella un'azienda.
    /// </summary>
    // [Serializable]
    public class Company
    {
        /// <summary>
        /// Identificativo UNIVOCO
        /// </summary> 
        public int Id { get; set; }


        /// <summary>
        /// Nome della compagnia
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Partita IVA (italiana di 11 caratteri numerici)  
        /// </summary>
        public string? PartitaIva { get; set; }
        /// <summary>
        /// Codice fiscale (dell'azienda italiano da 11)  
        /// </summary>
        public string? CodiceFiscale { get; set; } 


        /// <summary>
        /// Elenco degli indirizzi associati
        /// </summary>
        public ICollection<Address>? Addresses { get; set; }
        /// <summary>
        /// Elenco dei contatti associati
        /// </summary>
        public ICollection<Contact>? Contacts { get; set; }
        /// <summary>
        /// Elenco dei prodotti associati
        /// </summary>
        public ICollection<Product>? Products { get; set; }

        /// <summary>
        /// Questo costruttore che inizializza l'azienda/compagnia
        /// </summary>
        public Company()
        {

        }
        /// <summary>
        /// Questo costruttore che inizializza l'azienda/compagnia
        /// (<paramref name="name"/>,<paramref name="address"/>, <paramref name="contact"/>).
        /// <example>
        /// Per esempio:
        /// <code>
        /// Address address 
        /// Company company = new company("MARIO ROSSI & COMPANY", "ITALIA");
        /// </code>
        /// <param name="code">Sigla della nazione</param>
        /// <param name="name">Nome della nazione</param>
        /// </example>
        /// </summary>
        public Company(string name, Address address, Contact contact)
        {
            this.Name = name;
            this.Addresses = new List<Address> { address };
            this.Contacts = new List<Contact> { contact };
        }
    }
}
