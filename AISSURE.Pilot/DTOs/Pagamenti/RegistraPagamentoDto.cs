namespace AISSURE.Pilot.DTOs.Pagamenti;

/// <summary>
/// DTO per la registrazione di un pagamento (totale o parziale)
/// </summary>
public class RegistraPagamentoDto
{
    public int PagamentoId { get; set; }
    public decimal ImportoPagato { get; set; }
    public string MetodoPagamento { get; set; } = string.Empty; // Contanti, Bonifico, Carta, PayPal, Stripe, Satispay, POS
    public string? RiferimentoTransazione { get; set; }
}
