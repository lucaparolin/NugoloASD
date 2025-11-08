namespace AISSURE.Pilot.DTOs.Tesseramenti;

/// <summary>
/// DTO per l'aggiornamento di un tesseramento esistente
/// </summary>
public class TesseramentoUpdateDto
{
    public int TesseramentoId { get; set; }
    public int SocioId { get; set; }
    public int FederazioneId { get; set; }

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

    public string StatoTesseramento { get; set; } = "Attivo";

    public string? DocumentoTesseraUrl { get; set; }

    public string? Note { get; set; }
}
