using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NugoloASD.Web.DTOs.Soci;
using NugoloASD.Web.Mappers;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Services.Interfaces;

namespace NugoloASD.Web.API;

/// <summary>
/// Controller API per la gestione dei Soci
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SociController : ControllerBase
{
    private readonly ISocioRepository _socioRepository;
    private readonly ITenantService _tenantService;
    private readonly ILogger<SociController> _logger;

    public SociController(
        ISocioRepository socioRepository,
        ITenantService tenantService,
        ILogger<SociController> logger)
    {
        _socioRepository = socioRepository;
        _tenantService = tenantService;
        _logger = logger;
    }

    /// <summary>
    /// Ottiene la lista di tutti i soci
    /// </summary>
    /// <returns>Lista dei soci</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SocioListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var soci = await _socioRepository.GetAllAsync(tenantId);
            var sociDto = SocioMapper.ToListDto(soci);

            return Ok(sociDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei soci");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i dettagli di un socio specifico
    /// </summary>
    /// <param name="id">ID del socio</param>
    /// <returns>Dettagli del socio</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SocioDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var socio = await _socioRepository.GetByIdAsync(id, tenantId);

            if (socio == null)
            {
                return NotFound(new { message = "Socio non trovato" });
            }

            var socioDto = SocioMapper.ToDetailDto(socio);
            return Ok(socioDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero del socio {SocioId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Cerca soci per nome, cognome, CF, email o numero tessera
    /// </summary>
    /// <param name="searchTerm">Termine di ricerca</param>
    /// <returns>Lista dei soci trovati</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<SocioListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new { message = "Il termine di ricerca è obbligatorio" });
            }

            var tenantId = _tenantService.GetCurrentTenantId();
            var soci = await _socioRepository.SearchAsync(searchTerm, tenantId);
            var sociDto = SocioMapper.ToListDto(soci);

            return Ok(sociDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella ricerca soci con termine: {SearchTerm}", searchTerm);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i soci per tipo
    /// </summary>
    /// <param name="tipo">Tipo socio (Atleta, Istruttore, Dirigente, Genitore, Staff)</param>
    /// <returns>Lista dei soci del tipo specificato</returns>
    [HttpGet("tipo/{tipo}")]
    [ProducesResponseType(typeof(IEnumerable<SocioListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByTipo(string tipo)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var soci = await _socioRepository.GetByTipoAsync(tipo, tenantId);
            var sociDto = SocioMapper.ToListDto(soci);

            return Ok(sociDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei soci per tipo: {Tipo}", tipo);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i soci con certificato medico in scadenza
    /// </summary>
    /// <param name="giorni">Numero di giorni prima della scadenza (default 30)</param>
    /// <returns>Lista dei soci con certificato in scadenza</returns>
    [HttpGet("certificati-in-scadenza")]
    [ProducesResponseType(typeof(IEnumerable<SocioListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCertificatiInScadenza([FromQuery] int giorni = 30)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var soci = await _socioRepository.GetSociConCertificatoInScadenzaAsync(giorni, tenantId);
            var sociDto = SocioMapper.ToListDto(soci);

            return Ok(sociDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei soci con certificato in scadenza");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Crea un nuovo socio
    /// </summary>
    /// <param name="dto">Dati del nuovo socio</param>
    /// <returns>ID del socio creato</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] SocioCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica unicità codice fiscale
            var existing = await _socioRepository.GetByCodiceFiscaleAsync(dto.CodiceFiscale, tenantId);
            if (existing != null)
            {
                return BadRequest(new { message = "Esiste già un socio con questo codice fiscale" });
            }

            var socio = SocioMapper.ToEntity(dto);
            var socioId = await _socioRepository.InsertAsync(socio, tenantId);

            _logger.LogInformation("Creato nuovo socio {SocioId} per tenant {TenantId}", socioId, tenantId);
            return CreatedAtAction(nameof(GetById), new { id = socioId }, new { id = socioId, message = "Socio creato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione del socio");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Aggiorna un socio esistente
    /// </summary>
    /// <param name="id">ID del socio da aggiornare</param>
    /// <param name="dto">Nuovi dati del socio</param>
    /// <returns>Risultato dell'operazione</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] SocioUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.SocioId)
            {
                return BadRequest(new { message = "L'ID del socio non corrisponde" });
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica esistenza
            var existing = await _socioRepository.GetByIdAsync(id, tenantId);
            if (existing == null)
            {
                return NotFound(new { message = "Socio non trovato" });
            }

            // Verifica unicità codice fiscale (escludendo il socio corrente)
            var duplicateCF = await _socioRepository.GetByCodiceFiscaleAsync(dto.CodiceFiscale, tenantId);
            if (duplicateCF != null && duplicateCF.SocioId != id)
            {
                return BadRequest(new { message = "Esiste già un altro socio con questo codice fiscale" });
            }

            // Aggiorna entity
            SocioMapper.UpdateEntity(existing, dto);
            var updated = await _socioRepository.UpdateAsync(existing, tenantId);

            if (!updated)
            {
                return BadRequest(new { message = "Errore nell'aggiornamento del socio" });
            }

            _logger.LogInformation("Aggiornato socio {SocioId} per tenant {TenantId}", id, tenantId);
            return Ok(new { message = "Socio aggiornato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento del socio {SocioId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Elimina un socio (soft delete)
    /// </summary>
    /// <param name="id">ID del socio da eliminare</param>
    /// <returns>Risultato dell'operazione</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var deleted = await _socioRepository.DeleteAsync(id, tenantId);

            if (!deleted)
            {
                return NotFound(new { message = "Socio non trovato" });
            }

            _logger.LogInformation("Eliminato socio {SocioId} per tenant {TenantId}", id, tenantId);
            return Ok(new { message = "Socio eliminato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione del socio {SocioId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Verifica se esiste un socio con un dato codice fiscale
    /// </summary>
    /// <param name="cf">Codice fiscale da verificare</param>
    /// <returns>True se esiste, False altrimenti</returns>
    [HttpGet("check-cf/{cf}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckCodiceFiscale(string cf)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var socio = await _socioRepository.GetByCodiceFiscaleAsync(cf, tenantId);

            return Ok(new { exists = socio != null, socioId = socio?.SocioId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella verifica del codice fiscale");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }
}
