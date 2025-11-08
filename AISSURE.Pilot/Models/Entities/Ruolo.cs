using AISSURE.Pilot.Models.Base;

namespace AISSURE.Pilot.Models.Entities;

/// <summary>
/// Entity Ruolo
/// </summary>
public class Ruolo : BaseAuditEntity
{
    public int RuoloId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descrizione { get; set; }
    public int Livello { get; set; }

    // Navigation properties
    public virtual ICollection<Permesso>? Permessi { get; set; }
}
