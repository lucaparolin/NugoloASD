using AISSURE.Pilot.Models.Entities;

namespace AISSURE.Pilot.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione delle Associazioni (Tenant)
/// </summary>
public interface IAssociazioneRepository : IRepository<Associazione>
{
    /// <summary>
    /// Ottiene un'associazione per email
    /// </summary>
    Task<Associazione?> GetByEmailAsync(string email);

    /// <summary>
    /// Ottiene un'associazione per codice fiscale
    /// </summary>
    Task<Associazione?> GetByCodiceFiscaleAsync(string codiceFiscale);

    /// <summary>
    /// Ottiene un'associazione per dominio personalizzato
    /// </summary>
    Task<Associazione?> GetByDominioAsync(string dominio);

    /// <summary>
    /// Verifica se un'associazione è attiva
    /// </summary>
    Task<bool> IsActiveAsync(int associazioneId);
}
