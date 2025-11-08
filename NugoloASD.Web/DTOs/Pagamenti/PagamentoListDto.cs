namespace NugoloASD.Web.DTOs.Pagamenti;

/// <summary>
/// DTO per la lista dei pagamenti
/// </summary>
public class PagamentoListDto
{
    public int PagamentoId { get; set; }
    public int SocioId { get; set; }
    public string NomeSocio { get; set; } = string.Empty;
    public string CognomeSocio { get; set; } = string.Empty;
    public string Causale { get; set; } = string.Empty;
    public string? TipoPagamento { get; set; }
    public decimal Importo { get; set; }
    public decimal ImportoPagato { get; set; }
    public decimal ImportoResiduo { get; set; }
    public DateTime? DataScadenza { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string StatoPagamento { get; set; } = "In Attesa";
    public bool Scaduto => DataScadenza.HasValue && DataScadenza < DateTime.Today && StatoPagamento != "Pagato";
}
