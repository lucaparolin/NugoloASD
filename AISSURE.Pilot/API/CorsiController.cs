using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AISSURE.Pilot.DTOs.Corsi;
using AISSURE.Pilot.Mappers;
using AISSURE.Pilot.Repositories.Interfaces;
using AISSURE.Pilot.Services.Interfaces;

namespace AISSURE.Pilot.API;

/// <summary>
/// Controller API per la gestione dei Corsi
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CorsiController : ControllerBase
{
    private readonly ICorsoRepository _corsoRepository;
    private readonly ITenantService _tenantService;
    private readonly ILogger<CorsiController> _logger;

    public CorsiController(
        ICorsoRepository corsoRepository,
        ITenantService tenantService,
        ILogger<CorsiController> logger)
    {
        _corsoRepository = corsoRepository;
        _tenantService = tenantService;
        _logger = logger;
    }

    /// <summary>
    /// Ottiene la lista di tutti i corsi
    /// </summary>
    /// <returns>Lista dei corsi</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CorsoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var corsi = await _corsoRepository.GetAllAsync(tenantId);
            var corsiDto = CorsoMapper.ToListDto(corsi);

            return Ok(corsiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei corsi");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i dettagli di un corso specifico
    /// </summary>
    /// <param name="id">ID del corso</param>
    /// <returns>Dettagli del corso</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CorsoDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var corso = await _corsoRepository.GetByIdAsync(id, tenantId);

            if (corso == null)
            {
                return NotFound(new { message = "Corso non trovato" });
            }

            var corsoDto = CorsoMapper.ToDetailDto(corso);
            return Ok(corsoDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero del corso {CorsoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i corsi per categoria
    /// </summary>
    /// <param name="categoria">Categoria del corso</param>
    /// <returns>Lista dei corsi della categoria specificata</returns>
    [HttpGet("categoria/{categoria}")]
    [ProducesResponseType(typeof(IEnumerable<CorsoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByCategoria(string categoria)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var corsi = await _corsoRepository.GetByCategoriaAsync(categoria, tenantId);
            var corsiDto = CorsoMapper.ToListDto(corsi);

            return Ok(corsiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei corsi per categoria: {Categoria}", categoria);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i corsi per anno sportivo
    /// </summary>
    /// <param name="anno">Anno sportivo (es. 2024/2025)</param>
    /// <returns>Lista dei corsi dell'anno specificato</returns>
    [HttpGet("anno/{anno}")]
    [ProducesResponseType(typeof(IEnumerable<CorsoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByAnnoSportivo(string anno)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var corsi = await _corsoRepository.GetByAnnoSportivoAsync(anno, tenantId);
            var corsiDto = CorsoMapper.ToListDto(corsi);

            return Ok(corsiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei corsi per anno sportivo: {Anno}", anno);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i corsi attivi
    /// </summary>
    /// <returns>Lista dei corsi attivi</returns>
    [HttpGet("attivi")]
    [ProducesResponseType(typeof(IEnumerable<CorsoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCorsiAttivi()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var corsi = await _corsoRepository.GetCorsiAttiviAsync(tenantId);
            var corsiDto = CorsoMapper.ToListDto(corsi);

            return Ok(corsiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei corsi attivi");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i corsi con posti disponibili
    /// </summary>
    /// <returns>Lista dei corsi con posti disponibili</returns>
    [HttpGet("con-posti-disponibili")]
    [ProducesResponseType(typeof(IEnumerable<CorsoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCorsiConPostiDisponibili()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var corsi = await _corsoRepository.GetCorsiConPostiDisponibiliAsync(tenantId);
            var corsiDto = CorsoMapper.ToListDto(corsi);

            return Ok(corsiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei corsi con posti disponibili");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i corsi di un istruttore
    /// </summary>
    /// <param name="istruttoreId">ID dell'istruttore</param>
    /// <returns>Lista dei corsi dell'istruttore</returns>
    [HttpGet("istruttore/{istruttoreId}")]
    [ProducesResponseType(typeof(IEnumerable<CorsoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIstruttore(int istruttoreId)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var corsi = await _corsoRepository.GetByIstruttoreAsync(istruttoreId, tenantId);
            var corsiDto = CorsoMapper.ToListDto(corsi);

            return Ok(corsiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei corsi per istruttore {IstruttoreId}", istruttoreId);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Verifica la disponibilità di posti per un corso
    /// </summary>
    /// <param name="id">ID del corso</param>
    /// <returns>Informazioni sulla disponibilità</returns>
    [HttpGet("{id}/disponibilita")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckDisponibilita(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var corso = await _corsoRepository.GetByIdAsync(id, tenantId);

            if (corso == null)
            {
                return NotFound(new { message = "Corso non trovato" });
            }

            var postiLiberi = corso.PostiDisponibili - corso.PostiOccupati;
            var disponibile = postiLiberi > 0;

            return Ok(new
            {
                corsoId = corso.CorsoId,
                postiDisponibili = corso.PostiDisponibili,
                postiOccupati = corso.PostiOccupati,
                postiLiberi = postiLiberi,
                disponibile = disponibile,
                listaAttesaAttiva = corso.ListaAttesaAttiva
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella verifica disponibilità corso {CorsoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Crea un nuovo corso
    /// </summary>
    /// <param name="dto">Dati del nuovo corso</param>
    /// <returns>ID del corso creato</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CorsoCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantId = _tenantService.GetCurrentTenantId();
            var corso = CorsoMapper.ToEntity(dto);
            var corsoId = await _corsoRepository.InsertAsync(corso, tenantId);

            _logger.LogInformation("Creato nuovo corso {CorsoId} per tenant {TenantId}", corsoId, tenantId);
            return CreatedAtAction(nameof(GetById), new { id = corsoId }, new { id = corsoId, message = "Corso creato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione del corso");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Aggiorna un corso esistente
    /// </summary>
    /// <param name="id">ID del corso da aggiornare</param>
    /// <param name="dto">Nuovi dati del corso</param>
    /// <returns>Risultato dell'operazione</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] CorsoUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.CorsoId)
            {
                return BadRequest(new { message = "L'ID del corso non corrisponde" });
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica esistenza
            var existing = await _corsoRepository.GetByIdAsync(id, tenantId);
            if (existing == null)
            {
                return NotFound(new { message = "Corso non trovato" });
            }

            // Aggiorna entity
            CorsoMapper.UpdateEntity(existing, dto);
            var updated = await _corsoRepository.UpdateAsync(existing, tenantId);

            if (!updated)
            {
                return BadRequest(new { message = "Errore nell'aggiornamento del corso" });
            }

            _logger.LogInformation("Aggiornato corso {CorsoId} per tenant {TenantId}", id, tenantId);
            return Ok(new { message = "Corso aggiornato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento del corso {CorsoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Elimina un corso (soft delete)
    /// </summary>
    /// <param name="id">ID del corso da eliminare</param>
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

            var deleted = await _corsoRepository.DeleteAsync(id, tenantId);

            if (!deleted)
            {
                return NotFound(new { message = "Corso non trovato" });
            }

            _logger.LogInformation("Eliminato corso {CorsoId} per tenant {TenantId}", id, tenantId);
            return Ok(new { message = "Corso eliminato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione del corso {CorsoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }
}
