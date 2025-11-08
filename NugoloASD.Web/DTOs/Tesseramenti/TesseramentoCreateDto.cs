namespace NugoloASD.Web.DTOs.Tesseramenti;

/// <summary>
/// DTO per la creazione di un nuovo tesseramento
/// </summary>
public class TesseramentoCreateDto
{
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

    public string StatoTesseramento { get; set; } = "Attivo";

    public string? Note { get; set; }
}
