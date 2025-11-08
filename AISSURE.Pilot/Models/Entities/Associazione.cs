using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Models.Entities;

/// <summary>
/// Entity Associazione (Tenant)
/// </summary>
public class Associazione : BaseAuditEntity
{
    public int AssociazioneId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string RagioneSociale { get; set; } = string.Empty;
    public string? PartitaIVA { get; set; }
    public string CodiceFiscale { get; set; } = string.Empty;

    // Indirizzo
    public string? Indirizzo { get; set; }
    public string? Citta { get; set; }
    public string? CAP { get; set; }
    public string? Provincia { get; set; }
    public string? Regione { get; set; }
    public string Nazione { get; set; } = "Italia";

    // Contatti
    public string? Telefono { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PEC { get; set; }
    public string? SitoWeb { get; set; }

    // Branding
    public string? Logo { get; set; }
    public string? ColoriPrimari { get; set; }
    public string? DominioPersonalizzato { get; set; }

    // Configurazione
    public string? TipoSport { get; set; }
    public int NumeroMassimoSoci { get; set; } = 1000;
    public DateTime? DataScadenzaAbbonamento { get; set; }
    public string PianoAbbonamento { get; set; } = "Basic";

    // Stato
    public bool Attivo { get; set; } = true;
    public DateTime DataRegistrazione { get; set; } = DateTime.Now;
}
