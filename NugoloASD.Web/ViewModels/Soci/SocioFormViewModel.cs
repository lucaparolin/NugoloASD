using System.ComponentModel.DataAnnotations;

namespace NugoloASD.Web.ViewModels.Soci;

/// <summary>
/// ViewModel per form creazione/modifica socio
/// </summary>
public class SocioFormViewModel
{
    public int? SocioId { get; set; }

    [Required(ErrorMessage = "Il nome è obbligatorio")]
    [StringLength(100, ErrorMessage = "Il nome non può superare 100 caratteri")]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il cognome è obbligatorio")]
    [StringLength(100, ErrorMessage = "Il cognome non può superare 100 caratteri")]
    [Display(Name = "Cognome")]
    public string Cognome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Il codice fiscale è obbligatorio")]
    [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve essere di 16 caratteri")]
    [RegularExpression("^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$",
        ErrorMessage = "Formato codice fiscale non valido")]
    [Display(Name = "Codice Fiscale")]
    public string CodiceFiscale { get; set; } = string.Empty;

    [Required(ErrorMessage = "La data di nascita è obbligatoria")]
    [DataType(DataType.Date)]
    [Display(Name = "Data di Nascita")]
    public DateTime DataNascita { get; set; }

    [StringLength(100)]
    [Display(Name = "Luogo di Nascita")]
    public string? LuogoNascita { get; set; }

    [Required(ErrorMessage = "Il sesso è obbligatorio")]
    [Display(Name = "Sesso")]
    public string Sesso { get; set; } = "M";

    [EmailAddress(ErrorMessage = "Email non valida")]
    [StringLength(100)]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Numero di telefono non valido")]
    [StringLength(20)]
    [Display(Name = "Telefono")]
    public string? Telefono { get; set; }

    [Phone(ErrorMessage = "Numero di cellulare non valido")]
    [StringLength(20)]
    [Display(Name = "Cellulare")]
    public string? Cellulare { get; set; }

    [StringLength(200)]
    [Display(Name = "Indirizzo")]
    public string? Indirizzo { get; set; }

    [StringLength(50)]
    [Display(Name = "Città")]
    public string? Citta { get; set; }

    [StringLength(10)]
    [Display(Name = "CAP")]
    public string? CAP { get; set; }

    [StringLength(2)]
    [Display(Name = "Provincia")]
    public string? Provincia { get; set; }

    [Required(ErrorMessage = "Il tipo socio è obbligatorio")]
    [Display(Name = "Tipo Socio")]
    public string TipoSocio { get; set; } = "Tesserato";

    [Display(Name = "Minorenne")]
    public bool Minorenne { get; set; }

    [StringLength(100)]
    [Display(Name = "Nome Genitore")]
    public string? NomeGenitore { get; set; }

    [StringLength(100)]
    [Display(Name = "Cognome Genitore")]
    public string? CognomeGenitore { get; set; }

    [Phone]
    [StringLength(20)]
    [Display(Name = "Telefono Genitore")]
    public string? TelefonoGenitore { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data Scadenza Certificato Medico")]
    public DateTime? DataScadenzaCertificatoMedico { get; set; }

    [StringLength(500)]
    [Display(Name = "Note Mediche")]
    public string? NoteMediche { get; set; }

    [Required]
    [Display(Name = "Consenso Privacy")]
    public bool ConsensoPrivacy { get; set; }

    [Display(Name = "Consenso Marketing")]
    public bool ConsensoMarketing { get; set; }

    [Display(Name = "Attivo")]
    public bool Attivo { get; set; } = true;

    [StringLength(1000)]
    [Display(Name = "Note")]
    public string? Note { get; set; }
}
