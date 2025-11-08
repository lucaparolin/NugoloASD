using AISSURE.Pilot.Data;
using AISSURE.Pilot.Repositories.Interfaces;

namespace AISSURE.Pilot.Repositories.Implementations;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public PagamentoRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Task<object?> GetByIdAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare");
    }
}
