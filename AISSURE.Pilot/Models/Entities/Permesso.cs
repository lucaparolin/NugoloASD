using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Models.Entities;

/// <summary>
/// Entity Permesso
/// </summary>
public class Permesso : BaseAuditEntity
{
    public int PermessoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public string Risorsa { get; set; } = string.Empty;
    public string Azione { get; set; } = string.Empty;
}
