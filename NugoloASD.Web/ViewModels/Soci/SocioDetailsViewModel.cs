namespace NugoloASD.Web.ViewModels.Soci;

/// <summary>
/// ViewModel per i dettagli del socio
/// </summary>
public class SocioDetailsViewModel
{
    // Dati Anagrafici
    public int SocioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string NomeCompleto => $"{Nome} {Cognome}";
    public string CodiceFiscale { get; set; } = string.Empty;
    public DateTime DataNascita { get; set; }
    public int Eta => DateTime.Today.Year - DataNascita.Year -
        (DateTime.Today.DayOfYear < DataNascita.DayOfYear ? 1 : 0);
    public string? LuogoNascita { get; set; }
    public string Sesso { get; set; } = string.Empty;

    // Contatti
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Cellulare { get; set; }
    public string? Indirizzo { get; set; }
    public string? Citta { get; set; }
    public string? CAP { get; set; }
    public string? Provincia { get; set; }

    // Stato
    public string TipoSocio { get; set; } = string.Empty;
    public bool Attivo { get; set; }
    public bool Minorenne { get; set; }

    // Genitore (se minorenne)
    public string? NomeGenitore { get; set; }
    public string? CognomeGenitore { get; set; }
    public string? TelefonoGenitore { get; set; }

    // Certificato Medico
    public DateTime? DataScadenzaCertificatoMedico { get; set; }
    public bool CertificatoMedicoValido => DataScadenzaCertificatoMedico.HasValue &&
        DataScadenzaCertificatoMedico.Value >= DateTime.Today;
    public int? GiorniScadenzaCertificato => DataScadenzaCertificatoMedico.HasValue
        ? (DataScadenzaCertificatoMedico.Value - DateTime.Today).Days
        : null;
    public string? NoteMediche { get; set; }

    // Privacy
    public bool ConsensoPrivacy { get; set; }
    public bool ConsensoMarketing { get; set; }
    public DateTime? DataConsensoPrivacy { get; set; }

    // Note
    public string? Note { get; set; }

    // Audit
    public DateTime DataInserimento { get; set; }
    public DateTime? DataModifica { get; set; }

    // Statistiche
    public int NumeroCorsiAttivi { get; set; }
    public int NumeroTesseramentiAttivi { get; set; }
    public decimal TotalePagato { get; set; }
    public decimal TotaleDaPagare { get; set; }
}
