using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Services.Interfaces;

public interface ISocioService
{
    Task<IEnumerable<Socio>> GetAllSociAsync(int tenantId);
    Task<Socio?> GetSocioByIdAsync(int socioId, int tenantId);
    Task<int> CreateSocioAsync(Socio socio, int tenantId);
    Task<bool> UpdateSocioAsync(Socio socio, int tenantId);
    Task<bool> DeleteSocioAsync(int socioId, int tenantId);
}
