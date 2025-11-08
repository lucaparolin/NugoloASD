namespace AISSURE.Pilot.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione dei Tesseramenti
/// (Da implementare con entity Tesseramento)
/// </summary>
public interface ITesseramentoRepository
{
    // Placeholder - da implementare
    Task<object?> GetByIdAsync(int id, int tenantId);
}
