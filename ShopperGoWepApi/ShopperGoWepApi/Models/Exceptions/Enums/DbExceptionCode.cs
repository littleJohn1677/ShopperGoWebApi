// ===============================================================
// File name: DbExceptionCode.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

namespace ShopperGoWepApi.Models.Exceptions.Enums
{
    public enum DbExceptionCode
    {
        None = 0,
        OnDbSelect = 101, // Errore sull'interrogazione
        OnDbInsert = 102, // Errore in fase di inserimento
        OnDbUpdate = 102, // Errore in fase di aggiornamento
        OnDbDelete = 104, // Errore in fase di cancellazione
        OnUnexpected = 199 // Errore generico
    }
}
