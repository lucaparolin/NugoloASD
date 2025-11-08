namespace AISSURE.Pilot.DTOs.Pagamenti;

/// <summary>
/// DTO per la creazione di un nuovo pagamento
/// </summary>
public class PagamentoCreateDto
{
    public int SocioId { get; set; }

    // Causale
    public string Causale { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public string? TipoPagamento { get; set; }

    // Riferimenti
    public int? IscrizioneId { get; set; }
    public int? TesseramentoId { get; set; }
    public int? PrenotazioneId { get; set; }
    public int? AbbonamentoId { get; set; }

    // Importi
    public decimal Importo { get; set; }

    // Date
    public DateTime? DataScadenza { get; set; }

    // Rate
    public bool PagamentoRateale { get; set; } = false;
    public int? NumeroRata { get; set; }
    public int? TotaleRate { get; set; }

    public string? Note { get; set; }
}
