using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione dei Corsi
/// </summary>
public interface ICorsoRepository : IRepository<Corso>
{
    /// <summary>
    /// Ottiene tutti i corsi attivi
    /// </summary>
    Task<IEnumerable<Corso>> GetCorsiAttiviAsync(int tenantId);

    /// <summary>
    /// Ottiene tutti i corsi per anno sportivo
    /// </summary>
    Task<IEnumerable<Corso>> GetByAnnoSportivoAsync(string annoSportivo, int tenantId);

    /// <summary>
    /// Ottiene tutti i corsi per istruttore
    /// </summary>
    Task<IEnumerable<Corso>> GetByIstruttoreAsync(int istruttoreId, int tenantId);

    /// <summary>
    /// Ottiene tutti i corsi per categoria
    /// </summary>
    Task<IEnumerable<Corso>> GetByCategoriaAsync(string categoria, int tenantId);

    /// <summary>
    /// Verifica disponibilità posti
    /// </summary>
    Task<bool> HasPostiDisponibiliAsync(int corsoId, int tenantId);

    /// <summary>
    /// Incrementa posti occupati
    /// </summary>
    Task IncrementPostiOccupatiAsync(int corsoId, int tenantId);

    /// <summary>
    /// Decrementa posti occupati
    /// </summary>
    Task DecrementPostiOccupatiAsync(int corsoId, int tenantId);
}
