using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione degli Utenti
/// </summary>
public interface IUtenteRepository : IRepository<Utente>
{
    /// <summary>
    /// Ottiene un utente per username
    /// </summary>
    Task<Utente?> GetByUsernameAsync(string username, int tenantId);

    /// <summary>
    /// Ottiene un utente per email
    /// </summary>
    Task<Utente?> GetByEmailAsync(string email, int tenantId);

    /// <summary>
    /// Ottiene i ruoli di un utente
    /// </summary>
    Task<IEnumerable<Ruolo>> GetUserRolesAsync(int utenteId, int tenantId);

    /// <summary>
    /// Aggiorna la password dell'utente
    /// </summary>
    Task<bool> UpdatePasswordAsync(int utenteId, string passwordHash, string salt, int tenantId);

    /// <summary>
    /// Registra un tentativo di accesso fallito
    /// </summary>
    Task IncrementFailedLoginAttemptsAsync(int utenteId, int tenantId);

    /// <summary>
    /// Resetta i tentativ di accesso falliti
    /// </summary>
    Task ResetFailedLoginAttemptsAsync(int utenteId, int tenantId);

    /// <summary>
    /// Blocca l'account utente
    /// </summary>
    Task LockAccountAsync(int utenteId, int tenantId);

    /// <summary>
    /// Sblocca l'account utente
    /// </summary>
    Task UnlockAccountAsync(int utenteId, int tenantId);
}
