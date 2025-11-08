using AISSURE.Pilot.Data;
using AISSURE.Pilot.Models.Entities;
using AISSURE.Pilot.Repositories.Interfaces;

namespace AISSURE.Pilot.Repositories.Implementations;

/// <summary>
/// Repository per Utenti usando ADO.NET
/// </summary>
public class UtenteRepository : BaseRepository<Utente>, IUtenteRepository
{
    protected override string TableName => "Utenti";

    public UtenteRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override Task<IEnumerable<Utente>> GetAllAsync(int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<Utente?> GetByIdAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<Utente?> GetByUsernameAsync(string username, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<Utente?> GetByEmailAsync(string email, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Ruolo>> GetUserRolesAsync(int utenteId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<bool> UpdatePasswordAsync(int utenteId, string passwordHash, string salt, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task IncrementFailedLoginAttemptsAsync(int utenteId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task ResetFailedLoginAttemptsAsync(int utenteId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task LockAccountAsync(int utenteId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task UnlockAccountAsync(int utenteId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<int> InsertAsync(Utente entity, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<bool> UpdateAsync(Utente entity, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<bool> DeleteAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<bool> ExistsAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }
}
