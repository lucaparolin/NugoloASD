using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Services.Interfaces;

/// <summary>
/// Servizio per l'autenticazione e autorizzazione
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registra un nuovo utente
    /// </summary>
    Task<(bool Success, string Message, int? UtenteId)> RegisterAsync(
        string username,
        string email,
        string password,
        string nome,
        string cognome,
        int associazioneId);

    /// <summary>
    /// Effettua il login
    /// </summary>
    Task<(bool Success, string Message, string? Token, Utente? User)> LoginAsync(
        string usernameOrEmail,
        string password);

    /// <summary>
    /// Genera un token JWT
    /// </summary>
    string GenerateJwtToken(Utente user, IEnumerable<Ruolo> roles);

    /// <summary>
    /// Valida un token JWT
    /// </summary>
    Task<(bool IsValid, int? UtenteId, int? TenantId)> ValidateTokenAsync(string token);

    /// <summary>
    /// Richiede reset password
    /// </summary>
    Task<(bool Success, string Message)> RequestPasswordResetAsync(string email);

    /// <summary>
    /// Reset della password
    /// </summary>
    Task<(bool Success, string Message)> ResetPasswordAsync(string token, string newPassword);

    /// <summary>
    /// Cambia password
    /// </summary>
    Task<(bool Success, string Message)> ChangePasswordAsync(
        int utenteId,
        string oldPassword,
        string newPassword,
        int tenantId);

    /// <summary>
    /// Hash della password
    /// </summary>
    (string Hash, string Salt) HashPassword(string password);

    /// <summary>
    /// Verifica password
    /// </summary>
    bool VerifyPassword(string password, string hash, string salt);
}
