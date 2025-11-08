namespace AISSURE.Pilot.Services.Interfaces;

/// <summary>
/// Servizio per la gestione del contesto multi-tenant
/// </summary>
public interface ITenantService
{
    /// <summary>
    /// Ottiene l'ID del tenant corrente
    /// </summary>
    int GetCurrentTenantId();

    /// <summary>
    /// Imposta l'ID del tenant corrente
    /// </summary>
    void SetCurrentTenantId(int tenantId);

    /// <summary>
    /// Ottiene le informazioni del tenant corrente
    /// </summary>
    Task<Models.Entities.Associazione?> GetCurrentTenantAsync();

    /// <summary>
    /// Verifica se l'utente ha accesso al tenant specificato
    /// </summary>
    Task<bool> HasAccessToTenantAsync(int userId, int tenantId);
}
