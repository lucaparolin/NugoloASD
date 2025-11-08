using System.Data;
using AISSURE.Pilot.Data;
using AISSURE.Pilot.Models.Base;
using AISSURE.Pilot.Repositories.Interfaces;

namespace AISSURE.Pilot.Repositories.Implementations;

/// <summary>
/// Repository base con implementazione comune per tutte le entity
/// </summary>
public abstract class BaseRepository<T> : IRepository<T> where T : BaseAuditEntity
{
    protected readonly IDbConnectionFactory _connectionFactory;
    protected abstract string TableName { get; }

    protected BaseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
    }

    public abstract Task<IEnumerable<T>> GetAllAsync(int tenantId);
    public abstract Task<T?> GetByIdAsync(int id, int tenantId);
    public abstract Task<int> InsertAsync(T entity, int tenantId);
    public abstract Task<bool> UpdateAsync(T entity, int tenantId);
    public abstract Task<bool> DeleteAsync(int id, int tenantId);
    public abstract Task<bool> ExistsAsync(int id, int tenantId);

    /// <summary>
    /// Helper per impostare i campi di audit in inserimento
    /// </summary>
    protected void SetInsertAuditFields(T entity, string username)
    {
        entity.DataInserimento = DateTime.UtcNow;
        entity.UtenteInserimento = username;
    }

    /// <summary>
    /// Helper per impostare i campi di audit in modifica
    /// </summary>
    protected void SetUpdateAuditFields(T entity, string username)
    {
        entity.DataModifica = DateTime.UtcNow;
        entity.UtenteModifica = username;
    }
}
