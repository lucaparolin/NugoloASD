namespace NugoloASD.Web.DTOs.Soci;

/// <summary>
/// DTO per i dettagli completi di un socio
/// </summary>
public class SocioDetailDto
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
    public DateTime? DataIscrizione { get; set; }
    public string TipoSocio { get; set; } = "Atleta";
    public string StatoSocio { get; set; } = "Attivo";

    // Minore/Genitore
    public bool Minorenne { get; set; }
    public int? GenitoreId { get; set; }
    public string? NomeGenitore { get; set; }
    public string? CognomeGenitore { get; set; }

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
    public DateTime? DataConsensoPrivacy { get; set; }
    public bool ConsensoMarketing { get; set; }
    public bool ConsensoImmagini { get; set; }

    // Immagine
    public string? FotoUrl { get; set; }

    public string? Note { get; set; }

    // Audit
    public DateTime DataInserimento { get; set; }
    public DateTime? DataModifica { get; set; }
}
