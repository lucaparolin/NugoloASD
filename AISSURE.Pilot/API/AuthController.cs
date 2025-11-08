using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AISSURE.Pilot.Services.Interfaces;

namespace AISSURE.Pilot.API;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(
            request.Username,
            request.Email,
            request.Password,
            request.Nome,
            request.Cognome,
            request.AssociazioneId);

        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(new
        {
            message = result.Message,
            utenteId = result.UtenteId
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request.UsernameOrEmail, request.Password);

        if (!result.Success)
        {
            return Unauthorized(new { message = result.Message });
        }

        return Ok(new
        {
            message = result.Message,
            token = result.Token,
            user = new
            {
                result.User?.UtenteId,
                result.User?.Username,
                result.User?.Email,
                result.User?.Nome,
                result.User?.Cognome,
                result.User?.AssociazioneId
            }
        });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        var tenantIdClaim = User.FindFirst("TenantId");

        if (userIdClaim == null || tenantIdClaim == null)
        {
            return Unauthorized();
        }

        var userId = int.Parse(userIdClaim.Value);
        var tenantId = int.Parse(tenantIdClaim.Value);

        var result = await _authService.ChangePasswordAsync(
            userId,
            request.OldPassword,
            request.NewPassword,
            tenantId);

        if (!result.Success)
        {
            return BadRequest(new { message = result.Message });
        }

        return Ok(new { message = result.Message });
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequest request)
    {
        var result = await _authService.RequestPasswordResetAsync(request.Email);

        // Sempre OK per sicurezza (non rivelare se email esiste)
        return Ok(new { message = "Se l'email esiste, riceverai un link per il reset della password" });
    }
}

public record RegisterRequest(
    string Username,
    string Email,
    string Password,
    string Nome,
    string Cognome,
    int AssociazioneId);

public record LoginRequest(string UsernameOrEmail, string Password);

public record ChangePasswordRequest(string OldPassword, string NewPassword);

public record PasswordResetRequest(string Email);
