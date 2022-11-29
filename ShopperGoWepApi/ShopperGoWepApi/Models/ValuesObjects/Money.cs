// ===============================================================
// File name: Money.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using ShopperGoWepApi.Models.Enums;

namespace ShopperGoWepApi.Models.ValuesObjects
{
    /// <summary>
    /// Enum <c>Money</c> che definisce la tipologia di moneta
    /// </summary>
    public record Money // Uso i record perché voglio fare un confronto di un oggetto per ValueType
    {
        private decimal _amount = 0;
        /// <summary>
        /// Valore associato
        /// </summary>
        /// <exception cref="InvalidOperationException">Il valore non può essere negativo</exception>
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            init
            {
                if (value < 0)
                    throw new InvalidOperationException("Il valore non può essere negativo.");

                _amount = value;
            }
        }
        /// <summary>
        /// Definisce il tipo di valuta
        /// Può essere solo inizializzata, non è possibiel la modifica
        /// </summary>
        public Currency Currency
        {
            get; init;
        }


        /// <summary>
        /// Questo costruttore inizializza un nuova moneta
        /// (<paramref name="location"/>,<paramref name="citta"/>,<paramref name="nazioneSigla"/>,<paramref name="nomeNazione"/>).
        /// </summary>
        /// <param name="location">Indirizzo completo</param>
        /// <param name="citta">Citta</param>
        /// <param name="nazioneSigla">Sigla nazione</param>
        /// <param name="nomeNazione">Nome della nazione</param>
        public Money() : this(Currency.EUR, 0.00m) // Default
        {
        }
        /// <summary>
        /// Questo costruttore inizializza un nuova moneta
        /// (<paramref name="currency"/>,<paramref name="amount"/>).
        /// </summary>
        /// <param name="currency">Indirizzo completo</param>
        /// <param name="amount">Citta</param>
        public Money(Currency currency, decimal amount)
        {
            Amount = amount;
            Currency = currency;
        }


        /// <summary>
        /// ToString
        /// </summary>
        /// <returns>Ritorna la descrizione del prezzo</returns>
        public override string ToString()
        {
            return $"{Currency} {Amount:0.00}";
        }
    }
}
