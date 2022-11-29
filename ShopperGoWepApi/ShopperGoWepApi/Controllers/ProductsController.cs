// ===============================================================
// File name: ProductsController.cs
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
    /// Il controllo <c>ProductsController</c> gestice le operazioni per la gestione dei prodotti.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController: ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly EFProductsService _efProducts;
        private readonly EFCompaniesService _efCompanies;

        private bool ProductExists(int id)
        {
            return _efProducts.GetById(id) != null;
        }

        // --

        /// <summary>
        /// Selezione di un prodotto tramite l'identificativo univoco
        /// </summary>
        /// <param name="id">Identificativo del prodotto</param>
        /// <returns>Elenco dei prodotti</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Product), 200)]                // Successo
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        [ProducesResponseType(StatusCodes.Status404NotFound)]       // Dato non trovato
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _efProducts.Get(); // TODO: Inserire i parametri per la paginazione e il filtro.
            if (products == null)
                return BadRequest($"Nessun prodotto è stato inserito.");

            return products.ToList();
        }

        /// <summary>
        /// Selezione di un prodotto tramite l'identificativo univoco
        /// </summary>
        /// <param name="id">Identificativo del prodotto</param>
        /// <returns>Prodotto selezionato</returns>
        [HttpGet("id")]
        [ProducesResponseType(typeof(Product), 200)]                // Successo
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     // Errore sui dati
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        [ProducesResponseType(StatusCodes.Status404NotFound)]       // Dato non trovato
        public ActionResult<Product> Get(int id)
        {
            if (id <= 0)
                return BadRequest("Chiave di ricerca non valida.");

            var product = _efProducts.GetById(id);
            if (product == null)
                return BadRequest($"Nessun prodotto corrisponde all'identificativo {id}");

            return product;
        }

        /// <summary>
        /// Inserimento di una nuovo prodotto
        /// </summary>
        /// <param name="product">Prodotto da inserire</param>
        /// <returns>Esito dell'inserimento</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]        // Creato
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     // Errore sui dati
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        public async Task<IActionResult> Insert([FromBody] Product product)
        {
            _logger?.LogDebug("Inizio INSERIMENTO {Product}", product);

            if (product.CompanyId <= 0)
                return BadRequest("Identificativo della compagnia non è valido.");

            var company = await _efCompanies.GetByIdAsync(product.CompanyId);
            if (company == null)
                return BadRequest($"Nessuna compagnia corrisponde all'identificativo {product.CompanyId}");

            ProductValidator cv = new();
            var validation = cv.Validate(product);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            _logger?.LogDebug("=> VALIDAZIONE OK {Product}", product);

            try
            {
                // Tutti i testi in maiuscuolo
                Models.Utils.String.ToUpper(product);
                Models.Utils.String.ToUpper(product.Category);

                product.Company = company;

                await _efProducts.InsertAsync(product);
                _logger?.LogDebug("=> INSERIMENTO {Product}", product);

                _efProducts.SaveAsync();
                _logger?.LogDebug("=> PERSISTENZA {Product}", product);
            }
            catch (DbUpdateException ex)
            {
                Exception exe = new ProductException(DbExceptionCode.OnDbInsert,
                    "L'inserimento di un nuovo prodottto nella banca dati ha generato un errore inaspettato.", ex);

                _logger?.LogError(exe, "==> ERRORE INSERIMENTO {Product}", product);
                throw exe;
            }
            catch (Exception ex)
            {
                Exception exe = new CompanyException(DbExceptionCode.OnUnexpected,
                    "L'inserimento di una nuova compagnia nella banca dati ha prodotto un errore inaspettato.", ex);

                _logger?.LogError(exe, "==> ERRORE INSERIMENTO {Product}", product);
                throw exe;
            }

            _logger?.LogDebug("Fine INSERIMENTO {Product}", product);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        /// <summary>
        /// Aggiornamento del prodotto
        /// </summary>
        /// <param name="product">dati del prodotto da aggiornare</param>
        /// <returns>Esito dell'aggiornamento</returns>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]      // Aggiornamento effettuato con successo
        [ProducesResponseType(StatusCodes.Status404NotFound)]       // Compagnia NON trovata
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     // Errore sui dati
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        public async Task<IActionResult> Update(int id, Product product)
        {
            _logger?.LogDebug("Inizio AGGIORNAMENTO {Product}", product);

            if (id != product.Id)
                return BadRequest("Il codice identificativo non è valido.");

            var company = await _efCompanies.GetByIdAsync(product.CompanyId);
            if (company == null)
                return BadRequest($"Nessuna compagnia corrisponde all'identificativo {product.CompanyId}");

            ProductValidator cv = new();
            var validation = cv.Validate(product);
            if (!validation.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, validation.Errors);

            // TODO: Controllo se l'aggiornamento dei dati del prodottio, va in conflitto con un'altro prodotto già salvato

            _logger?.LogDebug("=> VALIDAZIONE OK {Product}", product);

            try
            {
                Models.Utils.String.ToUpper(product); // Tutti i testi in maiuscuolo

                _efProducts.Update(product);
                _logger?.LogDebug("=> AGGIORNAMENTO {Product}", product);

                _efProducts.SaveAsync();
                _logger?.LogDebug("=> PERSISTENZA {Product}", product);
            }
            catch (DbUpdateException ex)
            {
                if (!ProductExists(id))
                    return NotFound($"Il prodotto con identificativo {id} non è stata trovato.");
                else
                {
                    Exception exe = new ProductException(DbExceptionCode.OnDbUpdate,
                        "L'inserimento di una nuovo prodotto nella banca dati ha generato un errore inaspettato.", ex);

                    _logger?.LogError(exe, "==> ERRORE AGGIORNAMENTO {Company}", product);
                    throw exe;
                }
            }
            catch (Exception ex)
            {
                Exception exe = new ProductException(DbExceptionCode.OnUnexpected,
                    "L'inserimento di un nuovo prodotto nella banca dati ha generato un errore inaspettato.", ex);

                _logger?.LogError(exe, "==> ERRORE AGGIORNAMENTO {Company}", product);
                throw exe;
            }

            _logger?.LogDebug("Fine AGGIORNAMENTO {Company}", product);
            return NoContent();
        }

        /// <summary>
        /// Aggiornamento asincrono della compagnia
        /// </summary>
        /// <param name="company">dati della compagnia da aggiornare</param>
        /// <returns>Esito dell'aggiornamento</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]      // Aggiornamento effettuato con successo
        [ProducesResponseType(StatusCodes.Status404NotFound)]       // Compagnia NON trovata
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     // Errore sui dati
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   // Non autorizzato
        public async Task<IActionResult> Delete(int id)
        {
            _logger?.LogDebug("Inizio CANCELLAZIONE Product {Id}", id);

            if (id <= 0)
                return BadRequest("Chiave di ricerca non valida.");

            var product = _efProducts.GetById(id);
            if (product == null)
                return BadRequest($"Nessun prodotto corrisponde all'identificativo {id}");

            try
            {
                await _efProducts.DeleteAsync(id);
                _logger?.LogDebug("=> Cancellazione {Product}", product);

                _efProducts.SaveAsync();
                _logger?.LogDebug("=> PERSISTENZA {Product}", product);
            }
            catch (DbUpdateException ex)
            {
                Exception exe = new CompanyException(DbExceptionCode.OnDbDelete,
                        $"La cancellazione della compagnia con identificativo {id} ha prodotto un errore inaspettato.", ex);

                _logger?.LogError("=> PERSISTENZA {Product}", product);
                throw exe;
            }
            catch (Exception ex)
            {
                Exception exe = new CompanyException(DbExceptionCode.OnUnexpected,
                    $"L'inserimento di una nuova compagnia {id} ha prodotto un errore inaspettato.", ex);

                _logger?.LogError(exe, "==> ERRORE AGGIORNAMENTO {Product}", product);
                throw exe;
            }

            _logger?.LogDebug("Fine AGGIORNAMENTO {Product}", product);
            return NoContent();
        }

        // --

        /// <summary>
        /// Questo costruttore inizializza il controllo.
        /// (<paramref name="logger"/>, <paramref name="efProducts"/>, <paramref name="efCompanies"/>).
        /// </summary>
        /// <param name="logger">Dependency injection del servizio di tracciamento</param>
        /// <param name="efProducts">Dependency injection del servizio per interrogare i prodotti</param>
        /// <param name="efCompanies">Dependency injection del servizio per interrogare le compagnie</param>
        public ProductsController(ILogger<ProductsController> logger, 
            EFProductsService efProducts,
            EFCompaniesService efCompanies)
        {
            _logger = logger;
            this._efProducts = efProducts;
            this._efCompanies = efCompanies;
        }
    }
}
