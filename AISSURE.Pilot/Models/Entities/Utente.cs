using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Models.Entities;

/// <summary>
/// Entity Utente
/// </summary>
public class Utente : BaseAuditEntity
{
    public int UtenteId { get; set; }
    public int AssociazioneId { get; set; }

    // Credenziali
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;

    // Dati personali
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string? CodiceFiscale { get; set; }
    public DateTime? DataNascita { get; set; }
    public string? LuogoNascita { get; set; }

    // Contatti
    public string? Telefono { get; set; }
    public string? TelefonoCellulare { get; set; }

    // Sicurezza
    public bool TwoFactorEnabled { get; set; } = false;
    public string? TwoFactorSecret { get; set; }
    public DateTime? UltimoAccesso { get; set; }
    public int TentativiAccessoFalliti { get; set; } = 0;
    public bool AccountBloccato { get; set; } = false;
    public DateTime? DataBlocco { get; set; }

    // Stato
    public bool Attivo { get; set; } = true;
    public bool EmailConfermata { get; set; } = false;
    public string? TokenConfermaEmail { get; set; }
    public string? TokenResetPassword { get; set; }
    public DateTime? DataScadenzaTokenReset { get; set; }

    // Navigation properties (non mappate nel DB, ma utili nel codice)
    public virtual Associazione? Associazione { get; set; }
    public virtual ICollection<Ruolo>? Ruoli { get; set; }
}
