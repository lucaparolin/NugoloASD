namespace NugoloASD.Web.DTOs.Soci;

/// <summary>
/// DTO per l'aggiornamento di un socio esistente
/// </summary>
public class SocioUpdateDto
{
    public int SocioId { get; set; }

    // Dati anagrafici
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string CodiceFiscale { get; set; } = string.Empty;
    public DateTime DataNascita { get; set; }
    public string? LuogoNascita { get; set; }
    public string? ProvinciaNascita { get; set; }
    public string? NazioneNascita { get; set; }
    public string Sesso { get; set; } = "M";

    // Contatti
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Cellulare { get; set; }

    // Residenza
    public string? Indirizzo { get; set; }
    public string? Citta { get; set; }
    public string? Provincia { get; set; }
    public string? CAP { get; set; }
    public string? Nazione { get; set; }

    // Dati socio
    public string? NumeroTessera { get; set; }
    public string TipoSocio { get; set; } = "Atleta";
    public string StatoSocio { get; set; } = "Attivo";

    // Minore/Genitore
    public bool Minorenne { get; set; }
    public int? GenitoreId { get; set; }
    public string? NomeGenitore { get; set; }
    public string? CognomeGenitore { get; set; }
    public string? EmailGenitore { get; set; }
    public string? TelefonoGenitore { get; set; }

    // Documenti
    public string? NumeroDocumento { get; set; }
    public string? TipoDocumento { get; set; }
    public DateTime? DataScadenzaDocumento { get; set; }

    // Certificato medico
    public bool CertificatoMedicoValido { get; set; }
    public DateTime? DataCertificatoMedico { get; set; }
    public DateTime? DataScadenzaCertificato { get; set; }

    // Privacy
    public bool ConsensoPrivacy { get; set; }
    public bool ConsensoMarketing { get; set; }
    public bool ConsensoImmagini { get; set; }

    public string? Note { get; set; }
}
