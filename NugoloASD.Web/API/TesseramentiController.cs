using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NugoloASD.Web.DTOs.Tesseramenti;
using NugoloASD.Web.Mappers;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Services.Interfaces;

namespace NugoloASD.Web.API;

/// <summary>
/// Controller API per la gestione dei Tesseramenti
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TesseramentiController : ControllerBase
{
    private readonly ITesseramentoRepository _tesseramentoRepository;
    private readonly ISocioRepository _socioRepository;
    private readonly ITenantService _tenantService;
    private readonly ILogger<TesseramentiController> _logger;

    public TesseramentiController(
        ITesseramentoRepository tesseramentoRepository,
        ISocioRepository socioRepository,
        ITenantService tenantService,
        ILogger<TesseramentiController> logger)
    {
        _tesseramentoRepository = tesseramentoRepository;
        _socioRepository = socioRepository;
        _tenantService = tenantService;
        _logger = logger;
    }

    /// <summary>
    /// Ottiene la lista di tutti i tesseramenti
    /// </summary>
    /// <returns>Lista dei tesseramenti</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TesseramentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var tesseramenti = await _tesseramentoRepository.GetAllAsync(tenantId);
            var tesseramentiDto = TesseramentoMapper.ToListDto(tesseramenti);

            return Ok(tesseramentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei tesseramenti");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i dettagli di un tesseramento specifico
    /// </summary>
    /// <param name="id">ID del tesseramento</param>
    /// <returns>Dettagli del tesseramento</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TesseramentoDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var tesseramento = await _tesseramentoRepository.GetByIdAsync(id, tenantId);

            if (tesseramento == null)
            {
                return NotFound(new { message = "Tesseramento non trovato" });
            }

            var tesseramentoDto = TesseramentoMapper.ToDetailDto(tesseramento);
            return Ok(tesseramentoDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero del tesseramento {TesseramentoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i tesseramenti di un socio
    /// </summary>
    /// <param name="socioId">ID del socio</param>
    /// <returns>Lista dei tesseramenti del socio</returns>
    [HttpGet("socio/{socioId}")]
    [ProducesResponseType(typeof(IEnumerable<TesseramentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBySocio(int socioId)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var tesseramenti = await _tesseramentoRepository.GetBySocioAsync(socioId, tenantId);
            var tesseramentiDto = TesseramentoMapper.ToListDto(tesseramenti);

            return Ok(tesseramentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei tesseramenti per socio {SocioId}", socioId);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i tesseramenti per federazione
    /// </summary>
    /// <param name="federazioneId">ID della federazione</param>
    /// <returns>Lista dei tesseramenti della federazione</returns>
    [HttpGet("federazione/{federazioneId}")]
    [ProducesResponseType(typeof(IEnumerable<TesseramentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByFederazione(int federazioneId)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var tesseramenti = await _tesseramentoRepository.GetByFederazioneAsync(federazioneId, tenantId);
            var tesseramentiDto = TesseramentoMapper.ToListDto(tesseramenti);

            return Ok(tesseramentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei tesseramenti per federazione {FederazioneId}", federazioneId);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i tesseramenti per anno sportivo
    /// </summary>
    /// <param name="anno">Anno sportivo (es. 2024/2025)</param>
    /// <returns>Lista dei tesseramenti dell'anno specificato</returns>
    [HttpGet("anno/{anno}")]
    [ProducesResponseType(typeof(IEnumerable<TesseramentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByAnnoSportivo(string anno)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var tesseramenti = await _tesseramentoRepository.GetByAnnoSportivoAsync(anno, tenantId);
            var tesseramentiDto = TesseramentoMapper.ToListDto(tesseramenti);

            return Ok(tesseramentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei tesseramenti per anno sportivo: {Anno}", anno);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i tesseramenti attivi
    /// </summary>
    /// <returns>Lista dei tesseramenti attivi</returns>
    [HttpGet("attivi")]
    [ProducesResponseType(typeof(IEnumerable<TesseramentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTesseramentiAttivi()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var tesseramenti = await _tesseramentoRepository.GetTesseramentiAttiviAsync(tenantId);
            var tesseramentiDto = TesseramentoMapper.ToListDto(tesseramenti);

            return Ok(tesseramentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei tesseramenti attivi");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i tesseramenti in scadenza
    /// </summary>
    /// <param name="giorni">Numero di giorni prima della scadenza (default 30)</param>
    /// <returns>Lista dei tesseramenti in scadenza</returns>
    [HttpGet("in-scadenza")]
    [ProducesResponseType(typeof(IEnumerable<TesseramentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInScadenza([FromQuery] int giorni = 30)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var tesseramenti = await _tesseramentoRepository.GetTesseramentiInScadenzaAsync(giorni, tenantId);
            var tesseramentiDto = TesseramentoMapper.ToListDto(tesseramenti);

            return Ok(tesseramentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei tesseramenti in scadenza");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Verifica se un socio ha un tesseramento attivo per una federazione
    /// </summary>
    /// <param name="socioId">ID del socio</param>
    /// <param name="federazioneId">ID della federazione</param>
    /// <returns>Informazioni sul tesseramento attivo</returns>
    [HttpGet("check-attivo")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckTesseramentoAttivo(
        [FromQuery] int socioId,
        [FromQuery] int federazioneId)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var tesseramento = await _tesseramentoRepository.CheckTesseramentoAttivoAsync(
                socioId, federazioneId, tenantId);

            if (tesseramento == null)
            {
                return Ok(new
                {
                    attivo = false,
                    tesseramentoId = (int?)null,
                    dataScadenza = (DateTime?)null
                });
            }

            return Ok(new
            {
                attivo = true,
                tesseramentoId = tesseramento.TesseramentoId,
                numeroTessera = tesseramento.NumeroTessera,
                dataEmissione = tesseramento.DataEmissione,
                dataScadenza = tesseramento.DataScadenza,
                statoPagamento = tesseramento.Pagato ? "Pagato" : "Non Pagato"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella verifica tesseramento attivo");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Crea un nuovo tesseramento
    /// </summary>
    /// <param name="dto">Dati del nuovo tesseramento</param>
    /// <returns>ID del tesseramento creato</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] TesseramentoCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica esistenza socio
            var socio = await _socioRepository.GetByIdAsync(dto.SocioId, tenantId);
            if (socio == null)
            {
                return BadRequest(new { message = "Socio non trovato" });
            }

            // Verifica se esiste già un tesseramento attivo per la stessa federazione
            var existing = await _tesseramentoRepository.CheckTesseramentoAttivoAsync(
                dto.SocioId, dto.FederazioneId, tenantId);

            if (existing != null)
            {
                return BadRequest(new
                {
                    message = "Esiste già un tesseramento attivo per questa federazione",
                    tesseramentoId = existing.TesseramentoId,
                    numeroTessera = existing.NumeroTessera,
                    dataScadenza = existing.DataScadenza
                });
            }

            var tesseramento = TesseramentoMapper.ToEntity(dto);
            var tesseramentoId = await _tesseramentoRepository.InsertAsync(tesseramento, tenantId);

            _logger.LogInformation("Creato nuovo tesseramento {TesseramentoId} per tenant {TenantId}", tesseramentoId, tenantId);
            return CreatedAtAction(nameof(GetById), new { id = tesseramentoId }, new { id = tesseramentoId, message = "Tesseramento creato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione del tesseramento");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Aggiorna un tesseramento esistente
    /// </summary>
    /// <param name="id">ID del tesseramento da aggiornare</param>
    /// <param name="dto">Nuovi dati del tesseramento</param>
    /// <returns>Risultato dell'operazione</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] TesseramentoUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.TesseramentoId)
            {
                return BadRequest(new { message = "L'ID del tesseramento non corrisponde" });
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica esistenza
            var existing = await _tesseramentoRepository.GetByIdAsync(id, tenantId);
            if (existing == null)
            {
                return NotFound(new { message = "Tesseramento non trovato" });
            }

            // Verifica esistenza socio
            var socio = await _socioRepository.GetByIdAsync(dto.SocioId, tenantId);
            if (socio == null)
            {
                return BadRequest(new { message = "Socio non trovato" });
            }

            // Aggiorna entity
            TesseramentoMapper.UpdateEntity(existing, dto);
            var updated = await _tesseramentoRepository.UpdateAsync(existing, tenantId);

            if (!updated)
            {
                return BadRequest(new { message = "Errore nell'aggiornamento del tesseramento" });
            }

            _logger.LogInformation("Aggiornato tesseramento {TesseramentoId} per tenant {TenantId}", id, tenantId);
            return Ok(new { message = "Tesseramento aggiornato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento del tesseramento {TesseramentoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Elimina un tesseramento (soft delete)
    /// </summary>
    /// <param name="id">ID del tesseramento da eliminare</param>
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

            var deleted = await _tesseramentoRepository.DeleteAsync(id, tenantId);

            if (!deleted)
            {
                return NotFound(new { message = "Tesseramento non trovato" });
            }

            _logger.LogInformation("Eliminato tesseramento {TesseramentoId} per tenant {TenantId}", id, tenantId);
            return Ok(new { message = "Tesseramento eliminato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione del tesseramento {TesseramentoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }
}
