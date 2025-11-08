using AISSURE.Pilot.Data;
using AISSURE.Pilot.Repositories.Interfaces;

namespace AISSURE.Pilot.Repositories.Implementations;

public class ImpiantoRepository : IImpiantoRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ImpiantoRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Task<object?> GetByIdAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare");
    }
}
