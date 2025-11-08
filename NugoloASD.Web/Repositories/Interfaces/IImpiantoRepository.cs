using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione degli Impianti
/// </summary>
public interface IImpiantoRepository : IRepository<Impianto>
{
    /// <summary>
    /// Ottiene tutti gli impianti attivi
    /// </summary>
    Task<IEnumerable<Impianto>> GetImpiantiAttiviAsync(int tenantId);

    /// <summary>
    /// Ottiene tutti gli impianti per tipo
    /// </summary>
    Task<IEnumerable<Impianto>> GetByTipoImpiantoAsync(string tipoImpianto, int tenantId);

    /// <summary>
    /// Ottiene tutti gli impianti prenotabili
    /// </summary>
    Task<IEnumerable<Impianto>> GetImpiantiPrenotabiliAsync(int tenantId);

    /// <summary>
    /// Ottiene tutti gli impianti prenotabili online
    /// </summary>
    Task<IEnumerable<Impianto>> GetImpiantiPrenotabiliOnlineAsync(int tenantId);

    /// <summary>
    /// Cerca impianti per nome, tipo o città
    /// </summary>
    Task<IEnumerable<Impianto>> SearchAsync(string searchTerm, int tenantId);
}
