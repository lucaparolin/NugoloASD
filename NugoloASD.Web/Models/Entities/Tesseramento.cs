using NugoloASD.Web.Models.Base;

namespace NugoloASD.Web.Models.Entities;

/// <summary>
/// Entity Tesseramento - Tesseramenti federali dei soci
/// </summary>
public class Tesseramento : BaseAuditEntity
{
    public int TesseramentoId { get; set; }
    public int AssociazioneId { get; set; }
    public int SocioId { get; set; }
    public int FederazioneId { get; set; }

    public string NumeroTessera { get; set; } = string.Empty;
    public string AnnoSportivo { get; set; } = string.Empty;

    public string? TipoTessera { get; set; } // Atleta, Tecnico, Dirigente, Arbitro
    public string? Categoria { get; set; }
    public string? Qualifica { get; set; }

    public DateTime DataEmissione { get; set; }
    public DateTime DataScadenza { get; set; }

    public decimal? Importo { get; set; }
    public bool Pagato { get; set; } = false;
    public DateTime? DataPagamento { get; set; }

    // Stato
    public string StatoTesseramento { get; set; } = "Attivo"; // Attivo, Sospeso, Scaduto, Annullato

    // Documenti
    public string? DocumentoTesseraUrl { get; set; }

    public string? Note { get; set; }

    // Navigation properties
    public virtual Associazione? Associazione { get; set; }
    public virtual Socio? Socio { get; set; }
    public virtual Federazione? Federazione { get; set; }
}
