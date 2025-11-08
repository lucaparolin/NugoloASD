namespace NugoloASD.Web.DTOs.Tesseramenti;

/// <summary>
/// DTO per la lista dei tesseramenti
/// </summary>
public class TesseramentoListDto
{
    public int TesseramentoId { get; set; }
    public int SocioId { get; set; }
    public string NomeSocio { get; set; } = string.Empty;
    public string CognomeSocio { get; set; } = string.Empty;
    public int FederazioneId { get; set; }
    public string NomeFederazione { get; set; } = string.Empty;
    public string NumeroTessera { get; set; } = string.Empty;
    public string AnnoSportivo { get; set; } = string.Empty;
    public string? TipoTessera { get; set; }
    public DateTime DataEmissione { get; set; }
    public DateTime DataScadenza { get; set; }
    public string StatoTesseramento { get; set; } = "Attivo";
    public bool Scaduto => DataScadenza < DateTime.Today;
    public bool InScadenza => DataScadenza <= DateTime.Today.AddDays(30) && DataScadenza >= DateTime.Today;
}
