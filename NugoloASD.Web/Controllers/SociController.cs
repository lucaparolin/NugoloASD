using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NugoloASD.Web.Models.Entities;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Services.Interfaces;
using NugoloASD.Web.ViewModels.Soci;

namespace NugoloASD.Web.Controllers;

/// <summary>
/// Controller MVC per la gestione dei Soci
/// </summary>
[Authorize]
public class SociController : Controller
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
    /// Lista soci
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(FiltriSoci? filtri)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var soci = await _socioRepository.GetAllAsync(tenantId);

            // Applica filtri
            if (!string.IsNullOrWhiteSpace(filtri?.SearchTerm))
            {
                var searchTerm = filtri.SearchTerm.ToLower();
                soci = soci.Where(s =>
                    s.Nome.ToLower().Contains(searchTerm) ||
                    s.Cognome.ToLower().Contains(searchTerm) ||
                    s.CodiceFiscale.ToLower().Contains(searchTerm) ||
                    (s.Email?.ToLower().Contains(searchTerm) ?? false)
                ).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filtri?.TipoSocio))
            {
                soci = soci.Where(s => s.TipoSocio == filtri.TipoSocio).ToList();
            }

            if (filtri?.SoloAttivi == true)
            {
                soci = soci.Where(s => s.Attivo).ToList();
            }

            if (filtri?.CertificatoScaduto == true)
            {
                soci = soci.Where(s => !s.DataScadenzaCertificatoMedico.HasValue ||
                    s.DataScadenzaCertificatoMedico.Value < DateTime.Today).ToList();
            }

            // Crea ViewModel
            var viewModel = new SocioIndexViewModel
            {
                Soci = soci.Select(s => new SocioListItem
                {
                    SocioId = s.SocioId,
                    Nome = s.Nome,
                    Cognome = s.Cognome,
                    CodiceFiscale = s.CodiceFiscale,
                    Email = s.Email ?? "",
                    Telefono = s.Telefono ?? s.Cellulare ?? "",
                    TipoSocio = s.TipoSocio,
                    Attivo = s.Attivo,
                    CertificatoMedicoValido = s.DataScadenzaCertificatoMedico.HasValue &&
                        s.DataScadenzaCertificatoMedico.Value >= DateTime.Today,
                    DataScadenzaCertificato = s.DataScadenzaCertificatoMedico
                }).ToList(),
                Filtri = filtri ?? new FiltriSoci(),
                Statistiche = new StatisticheSoci
                {
                    TotaleSoci = soci.Count,
                    SociAttivi = soci.Count(s => s.Attivo),
                    CertificatiInScadenza = soci.Count(s =>
                        s.DataScadenzaCertificatoMedico.HasValue &&
                        s.DataScadenzaCertificatoMedico.Value >= DateTime.Today &&
                        s.DataScadenzaCertificatoMedico.Value <= DateTime.Today.AddDays(30))
                }
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel caricamento della lista soci");
            TempData["Error"] = "Errore nel caricamento dei soci";
            return View(new SocioIndexViewModel());
        }
    }

    /// <summary>
    /// Form creazione nuovo socio
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new SocioFormViewModel
        {
            DataNascita = DateTime.Today.AddYears(-18),
            Attivo = true
        };
        return View(viewModel);
    }

    /// <summary>
    /// Creazione nuovo socio
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SocioFormViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var tenantId = _tenantService.GetCurrentTenantId();

            // Verifica univocità codice fiscale
            var existing = await _socioRepository.GetByCodiceFiscaleAsync(model.CodiceFiscale, tenantId);
            if (existing != null)
            {
                ModelState.AddModelError("CodiceFiscale", "Esiste già un socio con questo codice fiscale");
                return View(model);
            }

            // Crea entity
            var socio = new Socio
            {
                Nome = model.Nome,
                Cognome = model.Cognome,
                CodiceFiscale = model.CodiceFiscale.ToUpper(),
                DataNascita = model.DataNascita,
                LuogoNascita = model.LuogoNascita,
                Sesso = model.Sesso,
                Email = model.Email,
                Telefono = model.Telefono,
                Cellulare = model.Cellulare,
                Indirizzo = model.Indirizzo,
                Citta = model.Citta,
                CAP = model.CAP,
                Provincia = model.Provincia,
                TipoSocio = model.TipoSocio,
                Minorenne = model.Minorenne,
                NomeGenitore = model.NomeGenitore,
                CognomeGenitore = model.CognomeGenitore,
                TelefonoGenitore = model.TelefonoGenitore,
                DataScadenzaCertificatoMedico = model.DataScadenzaCertificatoMedico,
                NoteMediche = model.NoteMediche,
                ConsensoPrivacy = model.ConsensoPrivacy,
                ConsensoMarketing = model.ConsensoMarketing,
                DataConsensoPrivacy = model.ConsensoPrivacy ? DateTime.UtcNow : null,
                Attivo = model.Attivo,
                Note = model.Note
            };

            var socioId = await _socioRepository.InsertAsync(socio, tenantId);

            _logger.LogInformation("Creato nuovo socio {SocioId} - {Nome} {Cognome}", socioId, socio.Nome, socio.Cognome);
            TempData["Success"] = $"Socio {socio.Nome} {socio.Cognome} creato con successo";

            return RedirectToAction(nameof(Details), new { id = socioId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella creazione del socio");
            ModelState.AddModelError("", "Errore nella creazione del socio");
            return View(model);
        }
    }

    /// <summary>
    /// Dettagli socio
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var socio = await _socioRepository.GetByIdAsync(id, tenantId);

            if (socio == null)
            {
                TempData["Error"] = "Socio non trovato";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new SocioDetailsViewModel
            {
                SocioId = socio.SocioId,
                Nome = socio.Nome,
                Cognome = socio.Cognome,
                CodiceFiscale = socio.CodiceFiscale,
                DataNascita = socio.DataNascita,
                LuogoNascita = socio.LuogoNascita,
                Sesso = socio.Sesso,
                Email = socio.Email,
                Telefono = socio.Telefono,
                Cellulare = socio.Cellulare,
                Indirizzo = socio.Indirizzo,
                Citta = socio.Citta,
                CAP = socio.CAP,
                Provincia = socio.Provincia,
                TipoSocio = socio.TipoSocio,
                Attivo = socio.Attivo,
                Minorenne = socio.Minorenne,
                NomeGenitore = socio.NomeGenitore,
                CognomeGenitore = socio.CognomeGenitore,
                TelefonoGenitore = socio.TelefonoGenitore,
                DataScadenzaCertificatoMedico = socio.DataScadenzaCertificatoMedico,
                NoteMediche = socio.NoteMediche,
                ConsensoPrivacy = socio.ConsensoPrivacy,
                ConsensoMarketing = socio.ConsensoMarketing,
                DataConsensoPrivacy = socio.DataConsensoPrivacy,
                Note = socio.Note,
                DataInserimento = socio.DataInserimento,
                DataModifica = socio.DataModifica
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel caricamento dei dettagli socio {SocioId}", id);
            TempData["Error"] = "Errore nel caricamento dei dettagli";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Form modifica socio
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var socio = await _socioRepository.GetByIdAsync(id, tenantId);

            if (socio == null)
            {
                TempData["Error"] = "Socio non trovato";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new SocioFormViewModel
            {
                SocioId = socio.SocioId,
                Nome = socio.Nome,
                Cognome = socio.Cognome,
                CodiceFiscale = socio.CodiceFiscale,
                DataNascita = socio.DataNascita,
                LuogoNascita = socio.LuogoNascita,
                Sesso = socio.Sesso,
                Email = socio.Email,
                Telefono = socio.Telefono,
                Cellulare = socio.Cellulare,
                Indirizzo = socio.Indirizzo,
                Citta = socio.Citta,
                CAP = socio.CAP,
                Provincia = socio.Provincia,
                TipoSocio = socio.TipoSocio,
                Minorenne = socio.Minorenne,
                NomeGenitore = socio.NomeGenitore,
                CognomeGenitore = socio.CognomeGenitore,
                TelefonoGenitore = socio.TelefonoGenitore,
                DataScadenzaCertificatoMedico = socio.DataScadenzaCertificatoMedico,
                NoteMediche = socio.NoteMediche,
                ConsensoPrivacy = socio.ConsensoPrivacy,
                ConsensoMarketing = socio.ConsensoMarketing,
                Attivo = socio.Attivo,
                Note = socio.Note
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel caricamento del form di modifica socio {SocioId}", id);
            TempData["Error"] = "Errore nel caricamento del form";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Modifica socio
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SocioFormViewModel model)
    {
        try
        {
            if (id != model.SocioId)
            {
                TempData["Error"] = "ID non valido";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var tenantId = _tenantService.GetCurrentTenantId();
            var socio = await _socioRepository.GetByIdAsync(id, tenantId);

            if (socio == null)
            {
                TempData["Error"] = "Socio non trovato";
                return RedirectToAction(nameof(Index));
            }

            // Verifica univocità codice fiscale
            if (socio.CodiceFiscale != model.CodiceFiscale.ToUpper())
            {
                var existing = await _socioRepository.GetByCodiceFiscaleAsync(model.CodiceFiscale, tenantId);
                if (existing != null)
                {
                    ModelState.AddModelError("CodiceFiscale", "Esiste già un socio con questo codice fiscale");
                    return View(model);
                }
            }

            // Aggiorna entity
            socio.Nome = model.Nome;
            socio.Cognome = model.Cognome;
            socio.CodiceFiscale = model.CodiceFiscale.ToUpper();
            socio.DataNascita = model.DataNascita;
            socio.LuogoNascita = model.LuogoNascita;
            socio.Sesso = model.Sesso;
            socio.Email = model.Email;
            socio.Telefono = model.Telefono;
            socio.Cellulare = model.Cellulare;
            socio.Indirizzo = model.Indirizzo;
            socio.Citta = model.Citta;
            socio.CAP = model.CAP;
            socio.Provincia = model.Provincia;
            socio.TipoSocio = model.TipoSocio;
            socio.Minorenne = model.Minorenne;
            socio.NomeGenitore = model.NomeGenitore;
            socio.CognomeGenitore = model.CognomeGenitore;
            socio.TelefonoGenitore = model.TelefonoGenitore;
            socio.DataScadenzaCertificatoMedico = model.DataScadenzaCertificatoMedico;
            socio.NoteMediche = model.NoteMediche;
            socio.ConsensoPrivacy = model.ConsensoPrivacy;
            socio.ConsensoMarketing = model.ConsensoMarketing;
            socio.Attivo = model.Attivo;
            socio.Note = model.Note;

            await _socioRepository.UpdateAsync(socio, tenantId);

            _logger.LogInformation("Aggiornato socio {SocioId} - {Nome} {Cognome}", id, socio.Nome, socio.Cognome);
            TempData["Success"] = $"Socio {socio.Nome} {socio.Cognome} aggiornato con successo";

            return RedirectToAction(nameof(Details), new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nella modifica del socio {SocioId}", id);
            ModelState.AddModelError("", "Errore nella modifica del socio");
            return View(model);
        }
    }

    /// <summary>
    /// Eliminazione socio (soft delete)
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var deleted = await _socioRepository.DeleteAsync(id, tenantId);

            if (deleted)
            {
                _logger.LogInformation("Eliminato socio {SocioId}", id);
                TempData["Success"] = "Socio eliminato con successo";
            }
            else
            {
                TempData["Error"] = "Socio non trovato";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nell'eliminazione del socio {SocioId}", id);
            TempData["Error"] = "Errore nell'eliminazione del socio";
            return RedirectToAction(nameof(Index));
        }
    }
}
