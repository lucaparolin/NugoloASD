using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Models.Entities;

/// <summary>
/// Entity Impianto - Strutture sportive
/// </summary>
public class Impianto : BaseAuditEntity
{
    public int ImpiantoId { get; set; }
    public int AssociazioneId { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public string? TipoImpianto { get; set; } // Campo Calcio, Campo Tennis, Piscina, Palestra, Sala

    // Indirizzo
    public string? Indirizzo { get; set; }
    public string? Citta { get; set; }
    public string? CAP { get; set; }

    // Caratteristiche
    public int? Capienza { get; set; }
    public decimal? Superficie { get; set; }
    public string? UnitaMisura { get; set; } // mq, ettari
    public string? CopertoScoperto { get; set; } // Coperto, Scoperto, Misto

    // Prenotabilità
    public bool Prenotabile { get; set; } = true;
    public bool PrenotabileOnline { get; set; } = true;
    public int TempoMinimoPrenotazione { get; set; } = 60; // Minuti
    public int AnticipoPrevistaGiorni { get; set; } = 0;
    public int MassimoCancellazioneOre { get; set; } = 24;

    // Prezzi
    public decimal? TariffaOraria { get; set; }
    public decimal? TariffaGiornaliera { get; set; }

    // Immagini
    public string? ImmagineUrl { get; set; }

    // Stato
    public bool Attivo { get; set; } = true;

    // Navigation properties
    public virtual Associazione? Associazione { get; set; }
}
