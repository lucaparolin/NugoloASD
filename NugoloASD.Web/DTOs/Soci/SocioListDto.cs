namespace NugoloASD.Web.DTOs.Soci;

/// <summary>
/// DTO per la lista dei soci (campi essenziali)
/// </summary>
public class SocioListDto
{
    public int SocioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string CodiceFiscale { get; set; } = string.Empty;
    public DateTime DataNascita { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? NumeroTessera { get; set; }
    public string TipoSocio { get; set; } = "Atleta";
    public string StatoSocio { get; set; } = "Attivo";
    public bool CertificatoMedicoValido { get; set; }
    public DateTime? DataScadenzaCertificato { get; set; }
}
