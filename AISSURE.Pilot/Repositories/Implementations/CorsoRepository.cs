using AISSURE.Pilot.Data;
using AISSURE.Pilot.Models.Entities;
using AISSURE.Pilot.Repositories.Interfaces;

namespace AISSURE.Pilot.Repositories.Implementations;

/// <summary>
/// Repository per Corsi usando ADO.NET
/// </summary>
public class CorsoRepository : BaseRepository<Corso>, ICorsoRepository
{
    protected override string TableName => "Corsi";

    public CorsoRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override Task<IEnumerable<Corso>> GetAllAsync(int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<Corso?> GetByIdAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Corso>> GetCorsiAttiviAsync(int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Corso>> GetByAnnoSportivoAsync(string annoSportivo, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Corso>> GetByIstruttoreAsync(int istruttoreId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Corso>> GetByCategoriaAsync(string categoria, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<bool> HasPostiDisponibiliAsync(int corsoId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task IncrementPostiOccupatiAsync(int corsoId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task DecrementPostiOccupatiAsync(int corsoId, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<int> InsertAsync(Corso entity, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<bool> UpdateAsync(Corso entity, int tenantId)
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
