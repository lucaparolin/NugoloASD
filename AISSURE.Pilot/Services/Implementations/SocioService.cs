using AISSURE.Pilot.Models.Entities;
using AISSURE.Pilot.Repositories.Interfaces;
using AISSURE.Pilot.Services.Interfaces;

namespace AISSURE.Pilot.Services.Implementations;

public class SocioService : ISocioService
{
    private readonly ISocioRepository _socioRepository;

    public SocioService(ISocioRepository socioRepository)
    {
        _socioRepository = socioRepository;
    }

    public async Task<IEnumerable<Socio>> GetAllSociAsync(int tenantId)
    {
        return await _socioRepository.GetAllAsync(tenantId);
    }

    public async Task<Socio?> GetSocioByIdAsync(int socioId, int tenantId)
    {
        return await _socioRepository.GetByIdAsync(socioId, tenantId);
    }

    public async Task<int> CreateSocioAsync(Socio socio, int tenantId)
    {
        return await _socioRepository.InsertAsync(socio, tenantId);
    }

    public async Task<bool> UpdateSocioAsync(Socio socio, int tenantId)
    {
        return await _socioRepository.UpdateAsync(socio, tenantId);
    }

    public async Task<bool> DeleteSocioAsync(int socioId, int tenantId)
    {
        return await _socioRepository.DeleteAsync(socioId, tenantId);
    }
}
