namespace AISSURE.Pilot.Models.Base;

/// <summary>
/// Classe base per tutte le entity con campi di audit
/// </summary>
public abstract class BaseAuditEntity
{
    /// <summary>
    /// Data di inserimento del record
    /// </summary>
    public DateTime DataInserimento { get; set; }

    /// <summary>
    /// Data di ultima modifica del record
    /// </summary>
    public DateTime? DataModifica { get; set; }

    /// <summary>
    /// Username dell'utente che ha inserito il record
    /// </summary>
    public string UtenteInserimento { get; set; } = string.Empty;

    /// <summary>
    /// Username dell'utente che ha modificato il record
    /// </summary>
    public string? UtenteModifica { get; set; }
}
