// ===============================================================
// File name: CompanyException.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using ShopperGoWepApi.Models.Exceptions.Enums;

namespace ShopperGoWepApi.Models.Exceptions.Application
{
    /// <summary>
    /// Enum <c>CompanyException</c> definisce gli errori in fase di persistenza dei dati
    /// </summary>
    public class CompanyException : Exception
    {
        public DbExceptionCode Code { get; init; }

        public CompanyException(DbExceptionCode code) : base() {
            this.Code = code;
        }
        public CompanyException(DbExceptionCode code, string? message) : base(message) {
            this.Code = code;
        }
        public CompanyException(DbExceptionCode code, string? message, Exception? innerException) : 
            base(message, innerException) {
            this.Code = code;
        }
    }
}
