using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NugoloASD.Web.Models.Entities;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Services.Interfaces;

namespace NugoloASD.Web.Services.Implementations;

/// <summary>
/// Implementazione servizio autenticazione
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUtenteRepository _utenteRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUtenteRepository utenteRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _utenteRepository = utenteRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<(bool Success, string Message, int? UtenteId)> RegisterAsync(
        string username,
        string email,
        string password,
        string nome,
        string cognome,
        int associazioneId)
    {
        try
        {
            // Verifica se username o email già esistono
            var existingUserByUsername = await _utenteRepository.GetByUsernameAsync(username, associazioneId);
            if (existingUserByUsername != null)
            {
                return (false, "Username già in uso", null);
            }

            var existingUserByEmail = await _utenteRepository.GetByEmailAsync(email, associazioneId);
            if (existingUserByEmail != null)
            {
                return (false, "Email già in uso", null);
            }

            // Hash password
            var (hash, salt) = HashPassword(password);

            // Crea nuovo utente
            var newUser = new Utente
            {
                AssociazioneId = associazioneId,
                Username = username,
                Email = email,
                PasswordHash = hash,
                Salt = salt,
                Nome = nome,
                Cognome = cognome,
                Attivo = true,
                EmailConfermata = false,
                TokenConfermaEmail = Guid.NewGuid().ToString(),
                UtenteInserimento = "SYSTEM"
            };

            var userId = await _utenteRepository.InsertAsync(newUser, associazioneId);

            _logger.LogInformation($"Nuovo utente registrato: {username} (ID: {userId})");

            return (true, "Registrazione completata con successo", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante la registrazione");
            return (false, "Errore durante la registrazione", null);
        }
    }

    public async Task<(bool Success, string Message, string? Token, Utente? User)> LoginAsync(
        string usernameOrEmail,
        string password)
    {
        try
        {
            // Cerca utente per username o email
            Utente? user = null;

            // Prova prima per email
            if (usernameOrEmail.Contains("@"))
            {
                // TODO: Implementare una ricerca senza tenantId per il login
                // Per ora, restituiamo un errore
                return (false, "Login non ancora implementato completamente", null, null);
            }
            else
            {
                // TODO: Stessa cosa per username
                return (false, "Login non ancora implementato completamente", null, null);
            }

            if (user == null)
            {
                _logger.LogWarning($"Tentativo di login fallito: utente {usernameOrEmail} non trovato");
                return (false, "Credenziali non valide", null, null);
            }

            // Verifica se account è bloccato
            if (user.AccountBloccato)
            {
                return (false, "Account bloccato. Contatta l'amministratore", null, null);
            }

            // Verifica password
            if (!VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                // Incrementa tentativo fallito
                await _utenteRepository.IncrementFailedLoginAttemptsAsync(user.UtenteId, user.AssociazioneId);

                // Blocca account dopo 5 tentativi
                if (user.TentativiAccessoFalliti >= 4)
                {
                    await _utenteRepository.LockAccountAsync(user.UtenteId, user.AssociazioneId);
                    return (false, "Account bloccato per troppi tentativi falliti", null, null);
                }

                return (false, "Credenziali non valide", null, null);
            }

            // Reset tentativi falliti
            await _utenteRepository.ResetFailedLoginAttemptsAsync(user.UtenteId, user.AssociazioneId);

            // Ottieni ruoli utente
            var roles = await _utenteRepository.GetUserRolesAsync(user.UtenteId, user.AssociazioneId);

            // Genera token JWT
            var token = GenerateJwtToken(user, roles);

            _logger.LogInformation($"Login effettuato con successo: {user.Username}");

            return (true, "Login effettuato con successo", token, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante il login");
            return (false, "Errore durante il login", null, null);
        }
    }

    public string GenerateJwtToken(Utente user, IEnumerable<Ruolo> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is missing");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UtenteId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("TenantId", user.AssociazioneId.ToString()),
            new Claim("Nome", user.Nome),
            new Claim("Cognome", user.Cognome)
        };

        // Aggiungi ruoli come claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Nome));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expirationMinutes = int.Parse(jwtSettings["ExpirationInMinutes"] ?? "1440");

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(bool IsValid, int? UtenteId, int? TenantId)> ValidateTokenAsync(string token)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is missing");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var utenteId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var tenantId = int.Parse(jwtToken.Claims.First(x => x.Type == "TenantId").Value);

            return (true, utenteId, tenantId);
        }
        catch
        {
            return (false, null, null);
        }
    }

    public async Task<(bool Success, string Message)> RequestPasswordResetAsync(string email)
    {
        try
        {
            // TODO: Implementare ricerca utente per email senza tenantId
            // Generare token reset
            // Inviare email con link reset

            return (false, "Funzionalità non ancora implementata");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante richiesta reset password");
            return (false, "Errore durante la richiesta");
        }
    }

    public async Task<(bool Success, string Message)> ResetPasswordAsync(string token, string newPassword)
    {
        try
        {
            // TODO: Implementare
            return (false, "Funzionalità non ancora implementata");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante reset password");
            return (false, "Errore durante il reset");
        }
    }

    public async Task<(bool Success, string Message)> ChangePasswordAsync(
        int utenteId,
        string oldPassword,
        string newPassword,
        int tenantId)
    {
        try
        {
            var user = await _utenteRepository.GetByIdAsync(utenteId, tenantId);
            if (user == null)
            {
                return (false, "Utente non trovato");
            }

            // Verifica vecchia password
            if (!VerifyPassword(oldPassword, user.PasswordHash, user.Salt))
            {
                return (false, "Password attuale non corretta");
            }

            // Hash nuova password
            var (hash, salt) = HashPassword(newPassword);

            // Aggiorna password
            await _utenteRepository.UpdatePasswordAsync(utenteId, hash, salt, tenantId);

            _logger.LogInformation($"Password cambiata per utente {utenteId}");

            return (true, "Password aggiornata con successo");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore durante cambio password");
            return (false, "Errore durante il cambio password");
        }
    }

    public (string Hash, string Salt) HashPassword(string password)
    {
        // Genera salt
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[32];
        rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        // Hash password con salt
        var hash = BCrypt.Net.BCrypt.HashPassword(password + salt);

        return (hash, salt);
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        return BCrypt.Net.BCrypt.Verify(password + salt, hash);
    }
}
