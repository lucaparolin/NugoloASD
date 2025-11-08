namespace AISSURE.Pilot.DTOs.Corsi;

/// <summary>
/// DTO per la lista dei corsi
/// </summary>
public class CorsoListDto
{
    public int CorsoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public string? Livello { get; set; }
    public DateTime DataInizio { get; set; }
    public DateTime DataFine { get; set; }
    public string? AnnoSportivo { get; set; }
    public int? PostiDisponibili { get; set; }
    public int PostiOccupati { get; set; }
    public int? PostiLiberi => PostiDisponibili.HasValue ? PostiDisponibili.Value - PostiOccupati : (int?)null;
    public bool ListaAttesaAttiva { get; set; }
    public decimal? PrezzoPieno { get; set; }
    public bool Attivo { get; set; }
    public bool PubblicatoOnline { get; set; }
}
