// ===============================================================
// File name: ProductException.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.29
// ===============================================================

using ShopperGoWepApi.Models.Exceptions.Enums;

namespace ShopperGoWepApi.Models.Exceptions.Application
{
    /// <summary>
    /// Enum <c>ProductException</c> definisce gli errori in fase di persistenza dei dati
    /// </summary>
    public class ProductException : Exception
    {
        public DbExceptionCode Code { get; init; }

        public ProductException(DbExceptionCode code) : base() {
            this.Code = code;
        }
        public ProductException(DbExceptionCode code, string? message) : base(message) {
            this.Code = code;
        }
        public ProductException(DbExceptionCode code, string? message, Exception? innerException) : 
            base(message, innerException) {
            this.Code = code;
        }
    }
}
