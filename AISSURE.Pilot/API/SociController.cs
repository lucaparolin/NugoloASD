using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AISSURE.Pilot.Services.Interfaces;
using AISSURE.Pilot.Models.Entities;

namespace AISSURE.Pilot.API;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SociController : ControllerBase
{
    private readonly ISocioService _socioService;
    private readonly ITenantService _tenantService;
    private readonly ILogger<SociController> _logger;

    public SociController(
        ISocioService socioService,
        ITenantService tenantService,
        ILogger<SociController> logger)
    {
        _socioService = socioService;
        _tenantService = tenantService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var soci = await _socioService.GetAllSociAsync(tenantId);

            return Ok(soci);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel recupero dei soci");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var socio = await _socioService.GetSocioByIdAsync(id, tenantId);

            if (socio == null)
            {
                return NotFound(new { message = "Socio non trovato" });
            }

            return Ok(socio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore nel recupero del socio {id}");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Socio socio)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            socio.AssociazioneId = tenantId;

            var socioId = await _socioService.CreateSocioAsync(socio, tenantId);

            return CreatedAtAction(nameof(GetById), new { id = socioId }, new { id = socioId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione del socio");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Socio socio)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica che il socio appartenga al tenant corrente
            var existingSocio = await _socioService.GetSocioByIdAsync(id, tenantId);
            if (existingSocio == null)
            {
                return NotFound(new { message = "Socio non trovato" });
            }

            socio.SocioId = id;
            socio.AssociazioneId = tenantId;

            var updated = await _socioService.UpdateSocioAsync(socio, tenantId);

            if (!updated)
            {
                return BadRequest(new { message = "Errore nell'aggiornamento" });
            }

            return Ok(new { message = "Socio aggiornato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore nell'aggiornamento del socio {id}");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var deleted = await _socioService.DeleteSocioAsync(id, tenantId);

            if (!deleted)
            {
                return NotFound(new { message = "Socio non trovato" });
            }

            return Ok(new { message = "Socio eliminato con successo" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Errore nell'eliminazione del socio {id}");
            return StatusCode(500, new { message = "Errore interno del server" });
        }
    }
}
