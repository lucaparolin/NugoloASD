using NugoloASD.Web.Services.Interfaces;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Services.Implementations;

/// <summary>
/// Implementazione servizio multi-tenant
/// </summary>
public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAssociazioneRepository _associazioneRepository;
    private int? _currentTenantId;

    public TenantService(
        IHttpContextAccessor httpContextAccessor,
        IAssociazioneRepository associazioneRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _associazioneRepository = associazioneRepository;
    }

    public int GetCurrentTenantId()
    {
        if (_currentTenantId.HasValue)
            return _currentTenantId.Value;

        // Prova a recuperare dal claim dell'utente autenticato
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == "TenantId");

            if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int tenantId))
            {
                _currentTenantId = tenantId;
                return tenantId;
            }
        }

        // Prova a recuperare dall'header
        if (httpContext?.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantHeader) == true)
        {
            if (int.TryParse(tenantHeader.FirstOrDefault(), out int tenantId))
            {
                _currentTenantId = tenantId;
                return tenantId;
            }
        }

        // Prova a recuperare dal sottodominio (es: associazione1.aissure.com)
        var host = httpContext?.Request.Host.Host;
        if (!string.IsNullOrEmpty(host))
        {
            var subdomain = host.Split('.')[0];
            // Qui potrebbe esserci una lookup nel DB per convertire il sottodominio in TenantId
            // Per ora, torniamo un errore
        }

        throw new InvalidOperationException("Tenant ID not found in current context");
    }

    public void SetCurrentTenantId(int tenantId)
    {
        _currentTenantId = tenantId;
    }

    public async Task<Associazione?> GetCurrentTenantAsync()
    {
        var tenantId = GetCurrentTenantId();
        return await _associazioneRepository.GetByIdAsync(tenantId, tenantId);
    }

    public async Task<bool> HasAccessToTenantAsync(int userId, int tenantId)
    {
        // Verifica se l'utente appartiene al tenant
        var userRepository = _httpContextAccessor.HttpContext?.RequestServices
            .GetService<IUtenteRepository>();

        if (userRepository == null)
            return false;

        var user = await userRepository.GetByIdAsync(userId, tenantId);
        return user != null && user.AssociazioneId == tenantId;
    }
}
