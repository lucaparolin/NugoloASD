using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Repositories.Interfaces;

/// <summary>
/// Repository per la gestione dei Tesseramenti
/// </summary>
public interface ITesseramentoRepository : IRepository<Tesseramento>
{
    /// <summary>
    /// Ottiene tutti i tesseramenti per socio
    /// </summary>
    Task<IEnumerable<Tesseramento>> GetBySocioAsync(int socioId, int tenantId);

    /// <summary>
    /// Ottiene tutti i tesseramenti per federazione
    /// </summary>
    Task<IEnumerable<Tesseramento>> GetByFederazioneAsync(int federazioneId, int tenantId);

    /// <summary>
    /// Ottiene tutti i tesseramenti per anno sportivo
    /// </summary>
    Task<IEnumerable<Tesseramento>> GetByAnnoSportivoAsync(string annoSportivo, int tenantId);

    /// <summary>
    /// Ottiene tutti i tesseramenti per stato
    /// </summary>
    Task<IEnumerable<Tesseramento>> GetByStatoAsync(string stato, int tenantId);

    /// <summary>
    /// Ottiene tesseramenti in scadenza entro un numero di giorni
    /// </summary>
    Task<IEnumerable<Tesseramento>> GetTesseramentiInScadenzaAsync(int giorniPrimaScadenza, int tenantId);

    /// <summary>
    /// Ottiene tesseramento per numero tessera
    /// </summary>
    Task<Tesseramento?> GetByNumeroTesseraAsync(string numeroTessera, int tenantId);

    /// <summary>
    /// Verifica se un socio ha un tesseramento attivo per una federazione
    /// </summary>
    Task<bool> HasTesseramentoAttivoAsync(int socioId, int federazioneId, int tenantId);
}
