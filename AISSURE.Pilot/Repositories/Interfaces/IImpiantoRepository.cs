using AISSURE.Pilot.Models.Entities;

namespace AISSURE.Pilot.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione degli Impianti
/// (Da implementare con entity Impianto)
/// </summary>
public interface IImpiantoRepository
{
    // Placeholder - da implementare
    Task<object?> GetByIdAsync(int id, int tenantId);
}
