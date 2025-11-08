using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.ViewModels.Soci;

/// <summary>
/// ViewModel per la lista soci
/// </summary>
public class SocioIndexViewModel
{
    public List<SocioListItem> Soci { get; set; } = new();
    public FiltriSoci Filtri { get; set; } = new();
    public StatisticheSoci Statistiche { get; set; } = new();
}

/// <summary>
/// Item per la lista soci
/// </summary>
public class SocioListItem
{
    public int SocioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string NomeCompleto => $"{Nome} {Cognome}";
    public string CodiceFiscale { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string TipoSocio { get; set; } = string.Empty;
    public bool Attivo { get; set; }
    public bool CertificatoMedicoValido { get; set; }
    public DateTime? DataScadenzaCertificato { get; set; }
}

/// <summary>
/// Filtri per la ricerca soci
/// </summary>
public class FiltriSoci
{
    public string? SearchTerm { get; set; }
    public string? TipoSocio { get; set; }
    public bool? SoloAttivi { get; set; }
    public bool? CertificatoScaduto { get; set; }
}

/// <summary>
/// Statistiche soci
/// </summary>
public class StatisticheSoci
{
    public int TotaleSoci { get; set; }
    public int SociAttivi { get; set; }
    public int CertificatiInScadenza { get; set; }
}
