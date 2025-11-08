using NugoloASD.Web.Models.Base;

namespace NugoloASD.Web.Models.Entities;

/// <summary>
/// Entity Federazione - Federazioni sportive
/// </summary>
public class Federazione : BaseAuditEntity
{
    public int FederazioneId { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public string? Descrizione { get; set; }

    public string? SitoWeb { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }

    // API Integration
    public string? APIEndpoint { get; set; }
    public string? APIKey { get; set; }
    public bool APIAbilitata { get; set; } = false;
}
