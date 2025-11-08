namespace AISSURE.Pilot.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione dei Pagamenti
/// (Da implementare con entity Pagamento)
/// </summary>
public interface IPagamentoRepository
{
    // Placeholder - da implementare
    Task<object?> GetByIdAsync(int id, int tenantId);
}
