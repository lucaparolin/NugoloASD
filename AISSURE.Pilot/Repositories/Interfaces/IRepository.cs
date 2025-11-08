using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Repositories.Interfaces;

/// <summary>
/// Interfaccia generica per i repository
/// </summary>
/// <typeparam name="T">Entity che eredita da BaseAuditEntity</typeparam>
public interface IRepository<T> where T : BaseAuditEntity
{
    /// <summary>
    /// Ottiene tutte le entità per il tenant specificato
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync(int tenantId);

    /// <summary>
    /// Ottiene un'entità per ID
    /// </summary>
    Task<T?> GetByIdAsync(int id, int tenantId);

    /// <summary>
    /// Inserisce una nuova entità
    /// </summary>
    Task<int> InsertAsync(T entity, int tenantId);

    /// <summary>
    /// Aggiorna un'entità esistente
    /// </summary>
    Task<bool> UpdateAsync(T entity, int tenantId);

    /// <summary>
    /// Elimina un'entità (soft delete o hard delete a seconda della logica)
    /// </summary>
    Task<bool> DeleteAsync(int id, int tenantId);

    /// <summary>
    /// Verifica se un'entità esiste
    /// </summary>
    Task<bool> ExistsAsync(int id, int tenantId);
}
