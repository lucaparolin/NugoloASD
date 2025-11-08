namespace NugoloASD.Web.DTOs.Tesseramenti;

/// <summary>
/// DTO per i dettagli completi di un tesseramento
/// </summary>
public class TesseramentoDetailDto
{
    public int TesseramentoId { get; set; }
    public int SocioId { get; set; }
    public string NomeSocio { get; set; } = string.Empty;
    public string CognomeSocio { get; set; } = string.Empty;
    public int FederazioneId { get; set; }
    public string NomeFederazione { get; set; } = string.Empty;
    public string SiglaFederazione { get; set; } = string.Empty;

    public string NumeroTessera { get; set; } = string.Empty;
    public string AnnoSportivo { get; set; } = string.Empty;

    public string? TipoTessera { get; set; }
    public string? Categoria { get; set; }
    public string? Qualifica { get; set; }

    public DateTime DataEmissione { get; set; }
    public DateTime DataScadenza { get; set; }

    public decimal? Importo { get; set; }
    public bool Pagato { get; set; }
    public DateTime? DataPagamento { get; set; }

    // Stato
    public string StatoTesseramento { get; set; } = "Attivo";
    public bool Scaduto => DataScadenza < DateTime.Today;
    public bool InScadenza => DataScadenza <= DateTime.Today.AddDays(30) && DataScadenza >= DateTime.Today;

    // Documenti
    public string? DocumentoTesseraUrl { get; set; }

    public string? Note { get; set; }

    // Audit
    public DateTime DataInserimento { get; set; }
    public DateTime? DataModifica { get; set; }
}
