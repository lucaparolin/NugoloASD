using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AISSURE.Pilot.DTOs.Pagamenti;
using AISSURE.Pilot.Mappers;
using AISSURE.Pilot.Repositories.Interfaces;
using AISSURE.Pilot.Services.Interfaces;

namespace AISSURE.Pilot.API;

/// <summary>
/// Controller API per la gestione dei Pagamenti
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PagamentiController : ControllerBase
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly ISocioRepository _socioRepository;
    private readonly ITenantService _tenantService;
    private readonly ILogger<PagamentiController> _logger;

    public PagamentiController(
        IPagamentoRepository pagamentoRepository,
        ISocioRepository socioRepository,
        ITenantService tenantService,
        ILogger<PagamentiController> logger)
    {
        _pagamentoRepository = pagamentoRepository;
        _socioRepository = socioRepository;
        _tenantService = tenantService;
        _logger = logger;
    }

    /// <summary>
    /// Ottiene la lista di tutti i pagamenti
    /// </summary>
    /// <returns>Lista dei pagamenti</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PagamentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var pagamenti = await _pagamentoRepository.GetAllAsync(tenantId);
            var pagamentiDto = PagamentoMapper.ToListDto(pagamenti);

            return Ok(pagamentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei pagamenti");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i dettagli di un pagamento specifico
    /// </summary>
    /// <param name="id">ID del pagamento</param>
    /// <returns>Dettagli del pagamento</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PagamentoDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var pagamento = await _pagamentoRepository.GetByIdAsync(id, tenantId);

            if (pagamento == null)
            {
                return NotFound(new { message = "Pagamento non trovato" });
            }

            var pagamentoDto = PagamentoMapper.ToDetailDto(pagamento);
            return Ok(pagamentoDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero del pagamento {PagamentoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i pagamenti di un socio
    /// </summary>
    /// <param name="socioId">ID del socio</param>
    /// <returns>Lista dei pagamenti del socio</returns>
    [HttpGet("socio/{socioId}")]
    [ProducesResponseType(typeof(IEnumerable<PagamentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBySocio(int socioId)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var pagamenti = await _pagamentoRepository.GetBySocioAsync(socioId, tenantId);
            var pagamentiDto = PagamentoMapper.ToListDto(pagamenti);

            return Ok(pagamentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei pagamenti per socio {SocioId}", socioId);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i pagamenti per stato
    /// </summary>
    /// <param name="stato">Stato pagamento (In Attesa, Parziale, Pagato, Annullato)</param>
    /// <returns>Lista dei pagamenti con lo stato specificato</returns>
    [HttpGet("stato/{stato}")]
    [ProducesResponseType(typeof(IEnumerable<PagamentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByStato(string stato)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var pagamenti = await _pagamentoRepository.GetByStatoAsync(stato, tenantId);
            var pagamentiDto = PagamentoMapper.ToListDto(pagamenti);

            return Ok(pagamentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei pagamenti per stato: {Stato}", stato);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i pagamenti scaduti
    /// </summary>
    /// <returns>Lista dei pagamenti scaduti</returns>
    [HttpGet("scaduti")]
    [ProducesResponseType(typeof(IEnumerable<PagamentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetScaduti()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var pagamenti = await _pagamentoRepository.GetPagamentiScadutiAsync(tenantId);
            var pagamentiDto = PagamentoMapper.ToListDto(pagamenti);

            return Ok(pagamentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei pagamenti scaduti");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene i pagamenti in scadenza
    /// </summary>
    /// <param name="giorni">Numero di giorni prima della scadenza (default 7)</param>
    /// <returns>Lista dei pagamenti in scadenza</returns>
    [HttpGet("in-scadenza")]
    [ProducesResponseType(typeof(IEnumerable<PagamentoListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetInScadenza([FromQuery] int giorni = 7)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var pagamenti = await _pagamentoRepository.GetPagamentiInScadenzaAsync(giorni, tenantId);
            var pagamentiDto = PagamentoMapper.ToListDto(pagamenti);

            return Ok(pagamentiDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei pagamenti in scadenza");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Ottiene statistiche sui totali incassati
    /// </summary>
    /// <param name="dataInizio">Data inizio periodo (opzionale)</param>
    /// <param name="dataFine">Data fine periodo (opzionale)</param>
    /// <returns>Totale incassato nel periodo</returns>
    [HttpGet("totale-incassato")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTotaleIncassato(
        [FromQuery] DateTime? dataInizio = null,
        [FromQuery] DateTime? dataFine = null)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var totale = await _pagamentoRepository.GetTotaleIncassatoAsync(
                dataInizio ?? DateTime.MinValue,
                dataFine ?? DateTime.MaxValue,
                tenantId);

            return Ok(new
            {
                dataInizio = dataInizio,
                dataFine = dataFine,
                totaleIncassato = totale
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel calcolo del totale incassato");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Crea un nuovo pagamento
    /// </summary>
    /// <param name="dto">Dati del nuovo pagamento</param>
    /// <returns>ID del pagamento creato</returns>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] PagamentoCreateDto dto)
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

            var pagamento = PagamentoMapper.ToEntity(dto);
            var pagamentoId = await _pagamentoRepository.InsertAsync(pagamento, tenantId);

            _logger.LogInformation("Creato nuovo pagamento {PagamentoId} per tenant {TenantId}", pagamentoId, tenantId);
            return CreatedAtAction(nameof(GetById), new { id = pagamentoId }, new { id = pagamentoId, message = "Pagamento creato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione del pagamento");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Registra un pagamento (totale o parziale)
    /// </summary>
    /// <param name="id">ID del pagamento</param>
    /// <param name="dto">Dati della registrazione pagamento</param>
    /// <returns>Risultato dell'operazione</returns>
    [HttpPost("{id}/registra")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegistraPagamento(int id, [FromBody] RegistraPagamentoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.PagamentoId)
            {
                return BadRequest(new { message = "L'ID del pagamento non corrisponde" });
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica esistenza pagamento
            var pagamento = await _pagamentoRepository.GetByIdAsync(id, tenantId);
            if (pagamento == null)
            {
                return NotFound(new { message = "Pagamento non trovato" });
            }

            // Verifica stato
            if (pagamento.StatoPagamento == "Pagato")
            {
                return BadRequest(new { message = "Il pagamento è già stato completato" });
            }

            if (pagamento.StatoPagamento == "Annullato")
            {
                return BadRequest(new { message = "Il pagamento è stato annullato" });
            }

            // Verifica importo
            var nuovoImportoPagato = pagamento.ImportoPagato + dto.ImportoPagato;
            if (nuovoImportoPagato > pagamento.Importo)
            {
                return BadRequest(new { message = "L'importo pagato supera l'importo totale" });
            }

            // Aggiorna pagamento
            pagamento.ImportoPagato = nuovoImportoPagato;
            pagamento.ImportoResiduo = pagamento.Importo - nuovoImportoPagato;
            pagamento.MetodoPagamento = dto.MetodoPagamento;
            pagamento.RiferimentoTransazione = dto.RiferimentoTransazione;
            pagamento.DataPagamento = DateTime.UtcNow;

            // Aggiorna stato
            if (pagamento.ImportoResiduo == 0)
            {
                pagamento.StatoPagamento = "Pagato";
            }
            else if (pagamento.ImportoPagato > 0)
            {
                pagamento.StatoPagamento = "Parziale";
            }

            var updated = await _pagamentoRepository.UpdateAsync(pagamento, tenantId);

            if (!updated)
            {
                return BadRequest(new { message = "Errore nella registrazione del pagamento" });
            }

            _logger.LogInformation(
                "Registrato pagamento {PagamentoId} - Importo: {Importo}, Stato: {Stato}",
                id, dto.ImportoPagato, pagamento.StatoPagamento);

            return Ok(new
            {
                message = "Pagamento registrato con successo",
                statoPagamento = pagamento.StatoPagamento,
                importoPagato = pagamento.ImportoPagato,
                importoResiduo = pagamento.ImportoResiduo
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella registrazione del pagamento {PagamentoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Aggiorna un pagamento esistente
    /// </summary>
    /// <param name="id">ID del pagamento da aggiornare</param>
    /// <param name="dto">Nuovi dati del pagamento</param>
    /// <returns>Risultato dell'operazione</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] PagamentoUpdateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.PagamentoId)
            {
                return BadRequest(new { message = "L'ID del pagamento non corrisponde" });
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica esistenza
            var existing = await _pagamentoRepository.GetByIdAsync(id, tenantId);
            if (existing == null)
            {
                return NotFound(new { message = "Pagamento non trovato" });
            }

            // Verifica esistenza socio
            var socio = await _socioRepository.GetByIdAsync(dto.SocioId, tenantId);
            if (socio == null)
            {
                return BadRequest(new { message = "Socio non trovato" });
            }

            // Aggiorna entity
            PagamentoMapper.UpdateEntity(existing, dto);
            var updated = await _pagamentoRepository.UpdateAsync(existing, tenantId);

            if (!updated)
            {
                return BadRequest(new { message = "Errore nell'aggiornamento del pagamento" });
            }

            _logger.LogInformation("Aggiornato pagamento {PagamentoId} per tenant {TenantId}", id, tenantId);
            return Ok(new { message = "Pagamento aggiornato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'aggiornamento del pagamento {PagamentoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    /// <summary>
    /// Elimina un pagamento (soft delete)
    /// </summary>
    /// <param name="id">ID del pagamento da eliminare</param>
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

            var deleted = await _pagamentoRepository.DeleteAsync(id, tenantId);

            if (!deleted)
            {
                return NotFound(new { message = "Pagamento non trovato" });
            }

            _logger.LogInformation("Eliminato pagamento {PagamentoId} per tenant {TenantId}", id, tenantId);
            return Ok(new { message = "Pagamento eliminato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione del pagamento {PagamentoId}", id);
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }
}
