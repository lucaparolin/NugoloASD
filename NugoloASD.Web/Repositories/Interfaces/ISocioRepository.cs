using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione dei Soci
/// </summary>
public interface ISocioRepository : IRepository<Socio>
{
    /// <summary>
    /// Ottiene un socio per codice fiscale
    /// </summary>
    Task<Socio?> GetByCodiceFiscaleAsync(string codiceFiscale, int tenantId);

    /// <summary>
    /// Ottiene un socio per numero tessera
    /// </summary>
    Task<Socio?> GetByNumeroTesseraAsync(string numeroTessera, int tenantId);

    /// <summary>
    /// Ottiene tutti i soci per tipo
    /// </summary>
    Task<IEnumerable<Socio>> GetByTipoAsync(string tipoSocio, int tenantId);

    /// <summary>
    /// Ottiene tutti i soci per stato
    /// </summary>
    Task<IEnumerable<Socio>> GetByStatoAsync(string statoSocio, int tenantId);

    /// <summary>
    /// Ottiene soci con certificato medico in scadenza
    /// </summary>
    Task<IEnumerable<Socio>> GetSociConCertificatoInScadenzaAsync(int giorniPrimaScadenza, int tenantId);

    /// <summary>
    /// Ricerca soci per nome, cognome o codice fiscale
    /// </summary>
    Task<IEnumerable<Socio>> SearchAsync(string searchTerm, int tenantId);
}
