using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Models.Entities;

/// <summary>
/// Entity Socio
/// </summary>
public class Socio : BaseAuditEntity
{
    public int SocioId { get; set; }
    public int AssociazioneId { get; set; }
    public int? UtenteId { get; set; }

    // Dati anagrafici
    public string Nome { get; set; } = string.Empty;
    public string Cognome { get; set; } = string.Empty;
    public string CodiceFiscale { get; set; } = string.Empty;
    public DateTime DataNascita { get; set; }
    public string? LuogoNascita { get; set; }
    public string? Sesso { get; set; }

    // Documenti identità
    public string? TipoDocumento { get; set; }
    public string? NumeroDocumento { get; set; }
    public DateTime? DataRilascioDocumento { get; set; }
    public DateTime? DataScadenzaDocumento { get; set; }
    public string? EnteRilascio { get; set; }

    // Residenza
    public string? IndirizzoResidenza { get; set; }
    public string? CittaResidenza { get; set; }
    public string? CAPResidenza { get; set; }
    public string? ProvinciaResidenza { get; set; }

    // Domicilio
    public string? IndirizzoDomicilio { get; set; }
    public string? CittaDomicilio { get; set; }
    public string? CAPDomicilio { get; set; }
    public string? ProvinciaDomicilio { get; set; }

    // Contatti
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? TelefonoCellulare { get; set; }

    // Informazioni socio
    public string? NumeroTessera { get; set; }
    public string TipoSocio { get; set; } = string.Empty;
    public DateTime? DataPrimaIscrizione { get; set; }
    public DateTime? DataUltimoRinnovo { get; set; }
    public string StatoSocio { get; set; } = "Attivo";

    // Minori - Gestione tutori
    public bool Minorenne { get; set; } = false;
    public int? GenitoreId { get; set; }
    public string? NomeGenitore1 { get; set; }
    public string? CognomeGenitore1 { get; set; }
    public string? TelefonoGenitore1 { get; set; }
    public string? EmailGenitore1 { get; set; }
    public string? NomeGenitore2 { get; set; }
    public string? CognomeGenitore2 { get; set; }
    public string? TelefonoGenitore2 { get; set; }
    public string? EmailGenitore2 { get; set; }

    // Privacy e consensi
    public bool ConsensoPrivacy { get; set; } = false;
    public DateTime? DataConsensoPrivacy { get; set; }
    public bool ConsensoMarketing { get; set; } = false;
    public bool ConsensoImmagini { get; set; } = false;

    // Foto
    public string? FotoUrl { get; set; }

    // Note
    public string? Note { get; set; }

    // Navigation properties
    public virtual Associazione? Associazione { get; set; }
    public virtual Utente? Utente { get; set; }
}
