namespace AISSURE.Pilot.DTOs.Corsi;

/// <summary>
/// DTO per i dettagli completi di un corso
/// </summary>
public class CorsoDetailDto
{
    public int CorsoId { get; set; }

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
    public int PostiOccupati { get; set; }
    public int? PostiLiberi => PostiDisponibili.HasValue ? PostiDisponibili.Value - PostiOccupati : (int?)null;
    public bool ListaAttesaAttiva { get; set; }

    // Istruttore
    public int? IstruttoreId { get; set; }
    public string? NomeIstruttore { get; set; }
    public string? CognomeIstruttore { get; set; }

    // Prezzi
    public decimal? PrezzoPieno { get; set; }
    public decimal? PrezzoRidotto { get; set; }
    public string? DescrizioneRiduzione { get; set; }

    // Immagine
    public string? ImmagineUrl { get; set; }

    // Stato
    public bool Attivo { get; set; }
    public bool PubblicatoOnline { get; set; }

    // Audit
    public DateTime DataInserimento { get; set; }
    public DateTime? DataModifica { get; set; }
}
