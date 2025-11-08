using NugoloASD.Web.DTOs.Pagamenti;
using NugoloASD.Web.Models.Entities;

namespace NugoloASD.Web.Mappers;

/// <summary>
/// Mapper per conversione Entity Pagamento <-> DTO
/// </summary>
public static class PagamentoMapper
{
    /// <summary>
    /// Converte Entity Pagamento in PagamentoListDto
    /// </summary>
    public static PagamentoListDto ToListDto(Pagamento entity, string? nomeSocio = null, string? cognomeSocio = null)
    {
        return new PagamentoListDto
        {
            PagamentoId = entity.PagamentoId,
            SocioId = entity.SocioId,
            NomeSocio = nomeSocio ?? entity.Socio?.Nome ?? string.Empty,
            CognomeSocio = cognomeSocio ?? entity.Socio?.Cognome ?? string.Empty,
            Causale = entity.Causale,
            TipoPagamento = entity.TipoPagamento,
            Importo = entity.Importo,
            ImportoPagato = entity.ImportoPagato,
            ImportoResiduo = entity.ImportoResiduo,
            DataScadenza = entity.DataScadenza,
            DataPagamento = entity.DataPagamento,
            StatoPagamento = entity.StatoPagamento
        };
    }

    /// <summary>
    /// Converte Entity Pagamento in PagamentoDetailDto
    /// </summary>
    public static PagamentoDetailDto ToDetailDto(Pagamento entity, string? nomeSocio = null, string? cognomeSocio = null)
    {
        return new PagamentoDetailDto
        {
            PagamentoId = entity.PagamentoId,
            SocioId = entity.SocioId,
            NomeSocio = nomeSocio ?? entity.Socio?.Nome ?? string.Empty,
            CognomeSocio = cognomeSocio ?? entity.Socio?.Cognome ?? string.Empty,
            Causale = entity.Causale,
            Descrizione = entity.Descrizione,
            TipoPagamento = entity.TipoPagamento,
            IscrizioneId = entity.IscrizioneId,
            TesseramentoId = entity.TesseramentoId,
            PrenotazioneId = entity.PrenotazioneId,
            AbbonamentoId = entity.AbbonamentoId,
            Importo = entity.Importo,
            ImportoPagato = entity.ImportoPagato,
            ImportoResiduo = entity.ImportoResiduo,
            MetodoPagamento = entity.MetodoPagamento,
            RiferimentoTransazione = entity.RiferimentoTransazione,
            DataScadenza = entity.DataScadenza,
            DataPagamento = entity.DataPagamento,
            StatoPagamento = entity.StatoPagamento,
            PagamentoRateale = entity.PagamentoRateale,
            NumeroRata = entity.NumeroRata,
            TotaleRate = entity.TotaleRate,
            Note = entity.Note,
            DataInserimento = entity.DataInserimento,
            DataModifica = entity.DataModifica
        };
    }

    /// <summary>
    /// Converte PagamentoCreateDto in Entity Pagamento
    /// </summary>
    public static Pagamento ToEntity(PagamentoCreateDto dto)
    {
        return new Pagamento
        {
            SocioId = dto.SocioId,
            Causale = dto.Causale,
            Descrizione = dto.Descrizione,
            TipoPagamento = dto.TipoPagamento,
            IscrizioneId = dto.IscrizioneId,
            TesseramentoId = dto.TesseramentoId,
            PrenotazioneId = dto.PrenotazioneId,
            AbbonamentoId = dto.AbbonamentoId,
            Importo = dto.Importo,
            ImportoPagato = 0, // Inizializzato a 0
            DataScadenza = dto.DataScadenza,
            StatoPagamento = "In Attesa", // Stato iniziale
            PagamentoRateale = dto.PagamentoRateale,
            NumeroRata = dto.NumeroRata,
            TotaleRate = dto.TotaleRate,
            Note = dto.Note
        };
    }

    /// <summary>
    /// Aggiorna Entity Pagamento con i dati da PagamentoUpdateDto
    /// </summary>
    public static void UpdateEntity(Pagamento entity, PagamentoUpdateDto dto)
    {
        entity.SocioId = dto.SocioId;
        entity.Causale = dto.Causale;
        entity.Descrizione = dto.Descrizione;
        entity.TipoPagamento = dto.TipoPagamento;
        entity.IscrizioneId = dto.IscrizioneId;
        entity.TesseramentoId = dto.TesseramentoId;
        entity.PrenotazioneId = dto.PrenotazioneId;
        entity.AbbonamentoId = dto.AbbonamentoId;
        entity.Importo = dto.Importo;
        entity.DataScadenza = dto.DataScadenza;
        entity.StatoPagamento = dto.StatoPagamento;
        entity.PagamentoRateale = dto.PagamentoRateale;
        entity.NumeroRata = dto.NumeroRata;
        entity.TotaleRate = dto.TotaleRate;
        entity.Note = dto.Note;
    }

    /// <summary>
    /// Converte lista di Entity in lista di ListDto
    /// </summary>
    public static IEnumerable<PagamentoListDto> ToListDto(IEnumerable<Pagamento> entities)
    {
        return entities.Select(e => ToListDto(e));
    }
}
