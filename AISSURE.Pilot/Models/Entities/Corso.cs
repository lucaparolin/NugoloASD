using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Models.Entities;

/// <summary>
/// Entity Corso
/// </summary>
public class Corso : BaseAuditEntity
{
    public int CorsoId { get; set; }
    public int AssociazioneId { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public string? Categoria { get; set; }
    public string? Livello { get; set; }

    // Periodo
    public DateTime DataInizio { get; set; }
    public DateTime DataFine { get; set; }
    public string? AnnoSportivo { get; set; }

    // Orari
    public string? GiorniSettimana { get; set; }
    public TimeSpan? OrarioInizio { get; set; }
    public TimeSpan? OrarioFine { get; set; }

    // Capacità
    public int? PostiDisponibili { get; set; }
    public int PostiOccupati { get; set; } = 0;
    public bool ListaAttesaAttiva { get; set; } = false;

    // Istruttore
    public int? IstruttoreId { get; set; }

    // Prezzi
    public decimal? PrezzoPieno { get; set; }
    public decimal? PrezzoRidotto { get; set; }
    public string? DescrizioneRiduzione { get; set; }

    // Immagine
    public string? ImmagineUrl { get; set; }

    // Stato
    public bool Attivo { get; set; } = true;
    public bool PubblicatoOnline { get; set; } = true;

    // Navigation properties
    public virtual Associazione? Associazione { get; set; }
    public virtual Socio? Istruttore { get; set; }
}
