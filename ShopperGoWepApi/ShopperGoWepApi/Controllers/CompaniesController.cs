// ===============================================================
// File name: CompaniesController.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ShopperGoWepApi.Models.Entities;
using ShopperGoWepApi.Models.Exceptions.Application;
using ShopperGoWepApi.Models.Exceptions.Enums;
using ShopperGoWepApi.Models.Services.Application.Companies;
using ShopperGoWepApi.Models.Validators;

namespace ShopperGoWepApi.Controllers
{
    /// <summary>
    /// Il controllo <c>CompaniesController</c> gestice le operazioni per la gestione delle compagnie.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly EFCompaniesService _efCompanies;

        private bool CompanyExists(int id)
        {
            return _efCompanies.GetById(id) != null;
        }

        // --

        /// <summary>
        /// Selezione di una compagnia tramite l'identificativo univoco
        /// </summary>
        /// <param name="id">Identificativo della compagnia</param>
        /// <returns>Elenco delle compagnie</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Company), 200)]                // Successo
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        [ProducesResponseType(StatusCodes.Status404NotFound)]       // Dato non trovato
        public ActionResult<IEnumerable<Company>> Get() 
        {
            var companies = _efCompanies.Get(); // TODO: Inserire i paramentri per la paginazione e il filtro.
            if (companies == null)
                return BadRequest($"Nessuna compagnia è stata inserita.");

            return companies.ToList();
        }

        /// <summary>
        /// Selezione di una compagnia tramite l'identificativo univoco
        /// </summary>
        /// <param name="id">Identificativo della compagnia</param>
        /// <returns>Compagnia</returns>
        [HttpGet("id")]
        [ProducesResponseType(typeof(Company), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     // Errore sui dati
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        [ProducesResponseType(StatusCodes.Status404NotFound)]       // Dato non trovato
        public ActionResult<Company> Get(int id)
        {
            if (id <= 0)
                return BadRequest("Chiave di ricerca non valida.");

            var company = _efCompanies.GetById(id);
            if (company == null)
                return BadRequest($"Nessuna compagnia corrisponde alla chiave di ricerca immessa {id}");

            return company;
        }

        /// <summary>
        /// Inserimento di una nuova compagnia
        /// </summary>
        /// <param name="company">Compagnia da inserire</param>
        /// <returns>Esito dell'inserimento</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]        // Creato
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     // Errore sui dati
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        public async Task<IActionResult> Insert([FromBody] Company company)
        {
            _logger?.LogDebug("Inizio INSERIMENTO {Company}", company);

            CompanyValidator cv = new();
            var validation = cv.Validate(company);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            _logger?.LogDebug("=> VALIDAZIONE OK {Company}", company);

            try
            {
                // Tutti i testi in maiuscuolo
                Models.Utils.String.ToUpper(company); 
                Models.Utils.String.ToUpper(company.Addresses);

                await _efCompanies.InsertAsync(company);
                _logger?.LogDebug("=> INSERIMENTO {Company}", company);

                _efCompanies.SaveAsync();
                _logger?.LogDebug("=> PERSISTENZA {Company}", company);
            }
            catch (DbUpdateException ex)
            {
                Exception exe = new CompanyException(DbExceptionCode.OnDbInsert,
                    "L'inserimento di una nuova compagnia nella banca dati ha prodotto un errore inaspettato.", ex);

                _logger?.LogError(exe, "==> ERRORE INSERIMENTO {Company}", company);
                throw exe;
            }
            catch (Exception ex)
            {
                Exception exe = new CompanyException(DbExceptionCode.OnUnexpected,
                    "L'inserimento di una nuova compagnia nella banca dati ha prodotto un errore inaspettato.", ex);

                _logger?.LogError(exe, "==> ERRORE INSERIMENTO {Company}", company);
                throw exe;
            }

            _logger?.LogDebug("Fine INSERIMENTO {Company}", company);
            return CreatedAtAction(nameof(Get), new { id = company.Id }, company);
        }

        /// <summary>
        /// Aggiornamento della compagnia
        /// </summary>
        /// <param name="company">dati della compagnia da aggiornare</param>
        /// <returns>Esito dell'aggiornamento</returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]      // Aggiornamento effettuato con successo
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     // Errore sui dati
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        [ProducesResponseType(StatusCodes.Status404NotFound)]       // Compagnia NON trovata
        public IActionResult Update(int id, Company company)
        {
            _logger?.LogDebug("Inizio AGGIORNAMENTO {Company}", company);

            if (id != company.Id)
                return BadRequest("Il codice identificativo non è valido.");

            CompanyValidator cv = new();
            var validation = cv.Validate(company);
            if (!validation.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validation.Errors);

            // TODO: Controllo se l'aggiornamento dei dati della compagnia, va in conflitto con un'altra compagnia già salvata

            _logger?.LogDebug("=> VALIDAZIONE OK {Company}", company);

            try
            {
                Models.Utils.String.ToUpper(company); // Tutti i testi in maiuscuolo

                _efCompanies.Update(company);
                _logger?.LogDebug("=> AGGIORNAMENTO {Company}", company);

                _efCompanies.SaveAsync();
                _logger?.LogDebug("=> PERSISTENZA {Company}", company);
            }
            catch (DbUpdateException ex)
            {
                if (!CompanyExists(id))
                    return NotFound($"La compagnia con identificativo {id} non è stata trovata.");
                else
                {
                    Exception exe = new CompanyException(DbExceptionCode.OnDbUpdate,
                        "L'inserimento di una nuova compagnia nella banca dati ha prodotto un errore inaspettato.", ex);

                    _logger?.LogError(exe, "==> ERRORE AGGIORNAMENTO {Company}", company);
                    throw exe;
                }
            }
            catch (Exception ex)
            {
                Exception exe = new CompanyException(DbExceptionCode.OnUnexpected,
                    "L'inserimento di una nuova compagnia nella banca dati ha prodotto un errore inaspettato.", ex);

                _logger?.LogError(exe, "==> ERRORE AGGIORNAMENTO {Company}", company);
                throw exe;
            }

            _logger?.LogDebug("Fine AGGIORNAMENTO {Company}", company);
            return NoContent();
        }

        /// <summary>
        /// Aggiornamento asincrono della compagnia
        /// </summary>
        /// <param name="company">dati della compagnia da aggiornare</param>
        /// <returns>Esito dell'aggiornamento</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]      // Aggiornamento effettuato con successo
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     // Errore sui dati
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        [ProducesResponseType(StatusCodes.Status404NotFound)]       // Compagnia NON trovata
        public async Task<IActionResult> Delete(int id)
        {
            _logger?.LogDebug("Inizio CANCELLAZIONE Company {Id}", id);

            if (id <= 0)
                return BadRequest("Chiave di ricerca non valida.");

            var company = _efCompanies.GetById(id);
            if (company == null)
                return BadRequest($"Nessuna compagnia corrisponde all'identificativo {id}");

            try
            {
                await _efCompanies.DeleteAsync(id);
                _logger?.LogDebug("=> Cancellazione {Company}", company);

                _efCompanies.SaveAsync();
                _logger?.LogDebug("=> PERSISTENZA {Company}", company);
            }
            catch (DbUpdateException ex)
            {
                Exception exe = new CompanyException(DbExceptionCode.OnDbDelete,
                        $"La cancellazione della compagnia con identificativo {id} ha prodotto un errore inaspettato.", ex);

                _logger?.LogError("=> PERSISTENZA {Company}", company);
                throw exe;
            }
            catch (Exception ex)
            {
                Exception exe = new CompanyException(DbExceptionCode.OnUnexpected,
                    $"L'inserimento di una nuova compagnia {id} ha prodotto un errore inaspettato.", ex);

                _logger?.LogError(exe, "==> ERRORE AGGIORNAMENTO {Company}", company);
                throw exe;
            }

            _logger?.LogDebug("Fine AGGIORNAMENTO {Company}", company);
            return NoContent();
        }

        // --

        /// <summary>
        /// Questo costruttore inizializza il controllo.
        /// (<paramref name="logger"/>).
        /// </summary>
        /// <param name="logger">Dependency injection del servizio di tracciamento</param>
        public CompaniesController(ILogger<CompaniesController> logger, EFCompaniesService efCompanies)
        {
            _logger = logger;
            this._efCompanies = efCompanies;
        }
    }
}
