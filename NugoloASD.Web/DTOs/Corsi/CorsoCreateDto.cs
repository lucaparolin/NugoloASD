namespace NugoloASD.Web.DTOs.Corsi;

/// <summary>
/// DTO per la creazione di un nuovo corso
/// </summary>
public class CorsoCreateDto
{
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
    public bool ListaAttesaAttiva { get; set; } = false;

    // Istruttore
    public int? IstruttoreId { get; set; }

    // Prezzi
    public decimal? PrezzoPieno { get; set; }
    public decimal? PrezzoRidotto { get; set; }
    public string? DescrizioneRiduzione { get; set; }

    // Stato
    public bool Attivo { get; set; } = true;
    public bool PubblicatoOnline { get; set; } = true;
}
