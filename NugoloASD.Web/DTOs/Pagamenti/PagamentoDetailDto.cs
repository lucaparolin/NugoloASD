namespace NugoloASD.Web.DTOs.Pagamenti;

/// <summary>
/// DTO per i dettagli completi di un pagamento
/// </summary>
public class PagamentoDetailDto
{
    public int PagamentoId { get; set; }
    public int SocioId { get; set; }
    public string NomeSocio { get; set; } = string.Empty;
    public string CognomeSocio { get; set; } = string.Empty;

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
    public decimal ImportoPagato { get; set; }
    public decimal ImportoResiduo { get; set; }

    // Metodo pagamento
    public string? MetodoPagamento { get; set; }
    public string? RiferimentoTransazione { get; set; }

    // Date
    public DateTime? DataScadenza { get; set; }
    public DateTime? DataPagamento { get; set; }

    // Stato
    public string StatoPagamento { get; set; } = "In Attesa";
    public bool Scaduto => DataScadenza.HasValue && DataScadenza < DateTime.Today && StatoPagamento != "Pagato";

    // Rate
    public bool PagamentoRateale { get; set; }
    public int? NumeroRata { get; set; }
    public int? TotaleRate { get; set; }

    public string? Note { get; set; }

    // Audit
    public DateTime DataInserimento { get; set; }
    public DateTime? DataModifica { get; set; }
}
