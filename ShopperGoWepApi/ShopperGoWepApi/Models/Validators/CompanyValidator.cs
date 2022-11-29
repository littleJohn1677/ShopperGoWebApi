// ===============================================================
// File name: CompanyValidator.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using ShopperGoWepApi.Models.Entities;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopperGoWepApi.Models.Validators
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        public const string CF_REGEX = "[A-Za-z]{6}[0-9LMNPQRSTUV]{2}[A-Za-z]{1}[0-9LMNPQRSTUV]{2}[A-Za-z]{1}[0-9LMNPQRSTUV]{3}[A-Za-z]{1}";
        public const string PI_REGEX = "[0-9]{11}";

        public CompanyValidator() 
        {
            RuleFor(company => company.Name).NotNull().NotEmpty().Length(1, 255)
                .WithMessage("Deve essere inserito il nome della compagnia, che non deve essere maggiore di 255 caratteri.");

            RuleFor(company => company.PartitaIva).Matches("^" + PI_REGEX + "$") // Partita IVA italiana
                .WithMessage("Partita IVA (italiana) non valida.");

            RuleFor(company => company.CodiceFiscale).Matches("^" + PI_REGEX + "|" + CF_REGEX + "$") // Codice fiscale italiana PG o PF
                .WithMessage("Codice fiscale (italiano) non valido.");

            RuleFor(company => company).Must(company => (company.Addresses != null && company.Addresses.Count > 0)
                && (company.Contacts != null && company.Contacts.Count > 0))
                    .WithMessage("Deve essere immesso almeno un indirizzo o un contatto.");

            RuleFor(company => company.Addresses)
                .Must(addresses => IsNotAddressesDuplicated(addresses))
                .WithMessage("Ci sono indirizzi duplicati nell'elenco.");

            RuleForEach(company => company.Addresses)
                .Must(address => address.Location != null && address.Location.Trim().Length > 0)
                .WithMessage("Deve essere indicata il luogo per l'indirizzo (posizione {CollectionIndex}).")
                .Must(address => address.City != null && address.City.Name.Trim().Length > 0)
                .WithMessage("Deve essere indicata la citta per l'indirizzo (posizione {CollectionIndex}).")
                .Must(address => address.City.PV != null)
                .WithMessage("Deve essere indicata la provincia per la citta associata all'indirizzo (posizione {CollectionIndex}).")
                .Must(address => address.City.PV != null && address.City.PV != "EE" &&
                    (address.City.Country == null ||
                    (address.City.Country != null && address.City.Country.Code != "IT") ||
                    (address.City.Country != null && address.City.Country.Code == "IT" && address.City.Country.Name == "ITALIA")))
                .Must(address => address.City.PV != "EE" || (address.City.PV == "EE" &&
                    address.City.Country != null &&
                    address.City.Country.Name != null &&
                    address.City.Country.Name.Trim().Length > 0) )
                .WithMessage("Deve essere indicata la descrizione della nazione per la citta associata all'indirizzo (posizione {CollectionIndex}).")
                .Must(address => address.City.PV != "EE" || (address.City.PV == "EE" &&
                    address.City.Country != null &&
                    address.City.Country.Code != null &&
                    address.City.Country.Code.Trim().Length > 0 &&
                    address.City.Country.Code.Trim().Length < 4))
                .WithMessage("Deve essere indicata la sigla della nazione per la citta associata all'indirizzo (posizione {CollectionIndex}).");

            RuleFor(company => company.Contacts)
                .Must(contacts => IsNotContactsDuplicated(contacts))
                .WithMessage("Ci sono contatti duplicati nell'elenco.");

            RuleForEach(company => company.Contacts)
                .Must(contact => contact.Value != null)
                .WithMessage("Deve essere indicato il valore per il (posizione {CollectionIndex}).");
        }

        /// <summary>
        /// Verifica se ci sono duplicati nei contatti. 
        /// (<paramref name="contacts"/>).
        /// </summary>
        /// <param name="contacts">Elenco degli indirizzi</param>
        /// <returns>True = Condizione verificata / False = Condizione NON verificata</returns>
        private bool IsNotContactsDuplicated(ICollection<Contact>? contacts)
        {
            if (contacts == null)
                return true;

            var duplicate = contacts
                .GroupBy(x => new { x.Type, x.Value })
                .Where(x => x.Skip(1).Any());

            return duplicate.Count() == 0;
        }

        /// <summary>
        /// Verifica se ci sono duplicati negli indirizzi. 
        /// (<paramref name="addresses"/>).
        /// </summary>
        /// <param name="addresses">Elenco degli indirizzi</param>
        /// <returns>True = Condizione verificata / False = Condizione NON verificata</returns>
        private bool IsNotAddressesDuplicated(ICollection<Address>? addresses)
        {
            if (addresses == null)
                return true;

            var duplicate = addresses
                .GroupBy(x => new { x.Location, x.City.Name, x.City.PV })
                .Where(x => x.Skip(1).Any());

            return duplicate.Count() == 0;
        }
    }
}
