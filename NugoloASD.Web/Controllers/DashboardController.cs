using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Services.Interfaces;
using NugoloASD.Web.ViewModels.Dashboard;

namespace NugoloASD.Web.Controllers;

/// <summary>
/// Controller per la dashboard principale
/// </summary>
[Authorize]
public class DashboardController : Controller
{
    private readonly ISocioRepository _socioRepository;
    private readonly ICorsoRepository _corsoRepository;
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly ITesseramentoRepository _tesseramentoRepository;
    private readonly ITenantService _tenantService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        ISocioRepository socioRepository,
        ICorsoRepository corsoRepository,
        IPagamentoRepository pagamentoRepository,
        ITesseramentoRepository tesseramentoRepository,
        ITenantService tenantService,
        ILogger<DashboardController> logger)
    {
        _socioRepository = socioRepository;
        _corsoRepository = corsoRepository;
        _pagamentoRepository = pagamentoRepository;
        _tesseramentoRepository = tesseramentoRepository;
        _tenantService = tenantService;
        _logger = logger;
    }

    /// <summary>
    /// Pagina dashboard principale
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            var viewModel = new DashboardViewModel();

            // Recupera KPI
            var soci = await _socioRepository.GetAllAsync(tenantId);
            var sociAttivi = soci.Count(s => s.Attivo);

            var corsi = await _corsoRepository.GetCorsiAttiviAsync(tenantId);

            var oggi = DateTime.Today;
            var primoGiornoMese = new DateTime(oggi.Year, oggi.Month, 1);
            var ultimoGiornoMese = primoGiornoMese.AddMonths(1).AddDays(-1);
            var incassiMese = await _pagamentoRepository.GetTotaleIncassatoAsync(
                primoGiornoMese, ultimoGiornoMese, tenantId);

            var tesseramentiInScadenza = await _tesseramentoRepository.GetTesseramentiInScadenzaAsync(30, tenantId);

            viewModel.Kpi = new KpiData
            {
                SociAttivi = sociAttivi,
                CorsiAttivi = corsi.Count(),
                IncassiMese = incassiMese,
                TesseramentiInScadenza = tesseramentiInScadenza.Count(),
                PercentualeVariazioneSoci = 5.2m, // TODO: Calcolare variazione reale
                PercentualeVariazioneIncassi = 12.8m // TODO: Calcolare variazione reale
            };

            // Attività recenti
            viewModel.AttivitaRecenti = new List<AttivitaRecente>
            {
                new AttivitaRecente
                {
                    Tipo = "Nuovo Socio",
                    Descrizione = "Nuova iscrizione completata",
                    Icona = "bi-person-plus",
                    ColorClass = "success",
                    Data = DateTime.Now.AddMinutes(-15)
                },
                new AttivitaRecente
                {
                    Tipo = "Pagamento",
                    Descrizione = "Pagamento corso ricevuto",
                    Icona = "bi-credit-card",
                    ColorClass = "primary",
                    Data = DateTime.Now.AddMinutes(-45)
                },
                new AttivitaRecente
                {
                    Tipo = "Tesseramento",
                    Descrizione = "Tesseramento rinnovato",
                    Icona = "bi-card-checklist",
                    ColorClass = "info",
                    Data = DateTime.Now.AddHours(-2)
                }
            };

            // Scadenze imminenti
            viewModel.ScadenzeImminenti = tesseramentiInScadenza.Take(5).Select(t =>
            {
                var giorniMancanti = (t.DataScadenza - DateTime.Today).Days;
                return new ScadenzaImminente
                {
                    Tipo = "Tesseramento",
                    Descrizione = $"Tesseramento {t.NumeroTessera}",
                    DataScadenza = t.DataScadenza,
                    GiorniMancanti = giorniMancanti,
                    PrioritaClass = giorniMancanti <= 7 ? "danger" : giorniMancanti <= 15 ? "warning" : "info"
                };
            }).ToList();

            // Dati grafici
            viewModel.GraficoIscrizioni = new GraficoData
            {
                Labels = new List<string> { "Gen", "Feb", "Mar", "Apr", "Mag", "Giu" },
                Values = new List<decimal> { 45, 52, 48, 65, 70, sociAttivi }
            };

            viewModel.GraficoIncassi = new GraficoData
            {
                Labels = new List<string> { "Gen", "Feb", "Mar", "Apr", "Mag", "Giu" },
                Values = new List<decimal> { 12500, 15200, 14800, 18900, 17500, incassiMese }
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore nel caricamento della dashboard");
            return View("Error");
        }
    }
}
