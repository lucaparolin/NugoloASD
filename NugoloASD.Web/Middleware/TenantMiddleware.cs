using NugoloASD.Web.Services.Interfaces;

namespace NugoloASD.Web.Middleware;

/// <summary>
/// Middleware per identificare e impostare il tenant corrente
/// </summary>
public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
    {
        // Prova a identificare il tenant da varie fonti
        int? tenantId = null;

        // 1. Header HTTP (per API)
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantHeader))
        {
            if (int.TryParse(tenantHeader.FirstOrDefault(), out int headerTenantId))
            {
                tenantId = headerTenantId;
            }
        }

        // 2. Claim dell'utente autenticato
        if (!tenantId.HasValue && context.User?.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = context.User.Claims
                .FirstOrDefault(c => c.Type == "TenantId");

            if (tenantClaim != null && int.TryParse(tenantClaim.Value, out int claimTenantId))
            {
                tenantId = claimTenantId;
            }
        }

        // 3. Sottodominio (es: associazione1.aissure.com)
        if (!tenantId.HasValue)
        {
            var host = context.Request.Host.Host;
            if (!string.IsNullOrEmpty(host))
            {
                var parts = host.Split('.');
                if (parts.Length > 2) // Ha un sottodominio
                {
                    var subdomain = parts[0];
                    // TODO: Implementare lookup sottodominio -> TenantId nel database
                    // Per ora ignoriamo
                }
            }
        }

        // Imposta il tenant se trovato
        if (tenantId.HasValue)
        {
            tenantService.SetCurrentTenantId(tenantId.Value);
        }

        await _next(context);
    }
}

/// <summary>
/// Extension method per registrare il middleware
/// </summary>
public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantMiddleware>();
    }
}
