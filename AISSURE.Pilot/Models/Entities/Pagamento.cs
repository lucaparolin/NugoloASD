using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Models.Entities;

/// <summary>
/// Entity Pagamento - Gestione pagamenti
/// </summary>
public class Pagamento : BaseAuditEntity
{
    public int PagamentoId { get; set; }
    public int AssociazioneId { get; set; }
    public int SocioId { get; set; }

    // Causale
    public string Causale { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public string? TipoPagamento { get; set; } // Iscrizione Corso, Tesseramento, Abbonamento, Prenotazione, Quota Sociale

    // Riferimenti
    public int? IscrizioneId { get; set; }
    public int? TesseramentoId { get; set; }
    public int? PrenotazioneId { get; set; }
    public int? AbbonamentoId { get; set; }

    // Importi
    public decimal Importo { get; set; }
    public decimal ImportoPagato { get; set; } = 0;
    public decimal ImportoResiduo => Importo - ImportoPagato; // Computed property

    // Metodo pagamento
    public string? MetodoPagamento { get; set; } // Contanti, Bonifico, Carta, PayPal, Stripe, Satispay, POS
    public string? RiferimentoTransazione { get; set; } // ID transazione gateway

    // Date
    public DateTime? DataScadenza { get; set; }
    public DateTime? DataPagamento { get; set; }

    // Stato
    public string StatoPagamento { get; set; } = "In Attesa"; // In Attesa, Pagato, Parziale, Scaduto, Annullato

    // Rate
    public bool PagamentoRateale { get; set; } = false;
    public int? NumeroRata { get; set; }
    public int? TotaleRate { get; set; }

    public string? Note { get; set; }

    // Navigation properties
    public virtual Associazione? Associazione { get; set; }
    public virtual Socio? Socio { get; set; }
}
