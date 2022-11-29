// ===============================================================
// File name: TokenMiddleware.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.29
// ===============================================================

namespace ShopperGoWepApi.Middleware
{
    /// <summary>
    /// Classe <c>TokenMiddleware</c> che gestisce il token del middleware per l'autenticazione tramite ApiKey
    /// </summary>
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private const string Token = "ApiKey";

        /// <summary>
        /// Verifica del token di autorizzazione.
        /// </summary>
        /// <param name="context">Contesto di esecuzione</param>
        /// <returns>Delegato della richiesta per il prossimo middleware</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(Token, out var key))
            {
                context.Response.StatusCode = 401; // Non autorizzato.
                await context.Response.WriteAsync("Inserire la chiave.");
                return;
            }

            // TODO: Creare sistema che genera TOKEN e li salva su banca dati.
            var settings = context.RequestServices.GetRequiredService<IConfiguration>();
            var secureKey = settings.GetValue<string>(Token);
            if (!secureKey.Equals(key))
            {
                context.Response.StatusCode = 401; // Non autorizzato.
                await context.Response.WriteAsync("Accesso non autorizzato.");
                return;
            }
            await _next(context);
        }

        /// <summary>
        /// Questo costruttore inizializza il middleware di autenticazione
        /// (<paramref name="next"/>).
        /// </summary>
        /// <param name="next">Delegato della richiesta</param>
        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }
    }
}
