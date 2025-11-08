using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione dei Pagamenti
/// </summary>
public interface IPagamentoRepository : IRepository<Pagamento>
{
    /// <summary>
    /// Ottiene tutti i pagamenti per socio
    /// </summary>
    Task<IEnumerable<Pagamento>> GetBySocioAsync(int socioId, int tenantId);

    /// <summary>
    /// Ottiene tutti i pagamenti per stato
    /// </summary>
    Task<IEnumerable<Pagamento>> GetByStatoAsync(string stato, int tenantId);

    /// <summary>
    /// Ottiene pagamenti in scadenza entro un numero di giorni
    /// </summary>
    Task<IEnumerable<Pagamento>> GetPagamentiInScadenzaAsync(int giorniPrimaScadenza, int tenantId);

    /// <summary>
    /// Ottiene pagamenti scaduti e non pagati
    /// </summary>
    Task<IEnumerable<Pagamento>> GetPagamentiScadutiAsync(int tenantId);

    /// <summary>
    /// Ottiene totale incassato per periodo
    /// </summary>
    Task<decimal> GetTotaleIncassatoAsync(DateTime dataInizio, DateTime dataFine, int tenantId);

    /// <summary>
    /// Ottiene totale da incassare per periodo
    /// </summary>
    Task<decimal> GetTotaleDaIncassareAsync(DateTime dataInizio, DateTime dataFine, int tenantId);

    /// <summary>
    /// Registra un pagamento (totale o parziale)
    /// </summary>
    Task<bool> RegistraPagamentoAsync(int pagamentoId, decimal importoPagato, string metodoPagamento, string? riferimentoTransazione, int tenantId);
}
