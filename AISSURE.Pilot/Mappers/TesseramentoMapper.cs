using AISSURE.Pilot.DTOs.Tesseramenti;
using AISSURE.Pilot.Models.Entities;

namespace AISSURE.Pilot.Mappers;

/// <summary>
/// Mapper per conversione Entity Tesseramento <-> DTO
/// </summary>
public static class TesseramentoMapper
{
    /// <summary>
    /// Converte Entity Tesseramento in TesseramentoListDto
    /// </summary>
    public static TesseramentoListDto ToListDto(Tesseramento entity,
        string? nomeSocio = null,
        string? cognomeSocio = null,
        string? nomeFederazione = null)
    {
        return new TesseramentoListDto
        {
            TesseramentoId = entity.TesseramentoId,
            SocioId = entity.SocioId,
            NomeSocio = nomeSocio ?? entity.Socio?.Nome ?? string.Empty,
            CognomeSocio = cognomeSocio ?? entity.Socio?.Cognome ?? string.Empty,
            FederazioneId = entity.FederazioneId,
            NomeFederazione = nomeFederazione ?? entity.Federazione?.Nome ?? string.Empty,
            NumeroTessera = entity.NumeroTessera,
            AnnoSportivo = entity.AnnoSportivo,
            TipoTessera = entity.TipoTessera,
            DataEmissione = entity.DataEmissione,
            DataScadenza = entity.DataScadenza,
            StatoTesseramento = entity.StatoTesseramento
        };
    }

    /// <summary>
    /// Converte Entity Tesseramento in TesseramentoDetailDto
    /// </summary>
    public static TesseramentoDetailDto ToDetailDto(Tesseramento entity,
        string? nomeSocio = null,
        string? cognomeSocio = null,
        string? nomeFederazione = null,
        string? siglaFederazione = null)
    {
        return new TesseramentoDetailDto
        {
            TesseramentoId = entity.TesseramentoId,
            SocioId = entity.SocioId,
            NomeSocio = nomeSocio ?? entity.Socio?.Nome ?? string.Empty,
            CognomeSocio = cognomeSocio ?? entity.Socio?.Cognome ?? string.Empty,
            FederazioneId = entity.FederazioneId,
            NomeFederazione = nomeFederazione ?? entity.Federazione?.Nome ?? string.Empty,
            SiglaFederazione = siglaFederazione ?? entity.Federazione?.Sigla ?? string.Empty,
            NumeroTessera = entity.NumeroTessera,
            AnnoSportivo = entity.AnnoSportivo,
            TipoTessera = entity.TipoTessera,
            Categoria = entity.Categoria,
            Qualifica = entity.Qualifica,
            DataEmissione = entity.DataEmissione,
            DataScadenza = entity.DataScadenza,
            Importo = entity.Importo,
            Pagato = entity.Pagato,
            DataPagamento = entity.DataPagamento,
            StatoTesseramento = entity.StatoTesseramento,
            DocumentoTesseraUrl = entity.DocumentoTesseraUrl,
            Note = entity.Note,
            DataInserimento = entity.DataInserimento,
            DataModifica = entity.DataModifica
        };
    }

    /// <summary>
    /// Converte TesseramentoCreateDto in Entity Tesseramento
    /// </summary>
    public static Tesseramento ToEntity(TesseramentoCreateDto dto)
    {
        return new Tesseramento
        {
            SocioId = dto.SocioId,
            FederazioneId = dto.FederazioneId,
            NumeroTessera = dto.NumeroTessera,
            AnnoSportivo = dto.AnnoSportivo,
            TipoTessera = dto.TipoTessera,
            Categoria = dto.Categoria,
            Qualifica = dto.Qualifica,
            DataEmissione = dto.DataEmissione,
            DataScadenza = dto.DataScadenza,
            Importo = dto.Importo,
            Pagato = dto.Pagato,
            DataPagamento = dto.DataPagamento,
            StatoTesseramento = dto.StatoTesseramento,
            Note = dto.Note
        };
    }

    /// <summary>
    /// Aggiorna Entity Tesseramento con i dati da TesseramentoUpdateDto
    /// </summary>
    public static void UpdateEntity(Tesseramento entity, TesseramentoUpdateDto dto)
    {
        entity.SocioId = dto.SocioId;
        entity.FederazioneId = dto.FederazioneId;
        entity.NumeroTessera = dto.NumeroTessera;
        entity.AnnoSportivo = dto.AnnoSportivo;
        entity.TipoTessera = dto.TipoTessera;
        entity.Categoria = dto.Categoria;
        entity.Qualifica = dto.Qualifica;
        entity.DataEmissione = dto.DataEmissione;
        entity.DataScadenza = dto.DataScadenza;
        entity.Importo = dto.Importo;
        entity.Pagato = dto.Pagato;
        entity.DataPagamento = dto.DataPagamento;
        entity.StatoTesseramento = dto.StatoTesseramento;
        entity.DocumentoTesseraUrl = dto.DocumentoTesseraUrl;
        entity.Note = dto.Note;
    }

    /// <summary>
    /// Converte lista di Entity in lista di ListDto
    /// </summary>
    public static IEnumerable<TesseramentoListDto> ToListDto(IEnumerable<Tesseramento> entities)
    {
        return entities.Select(e => ToListDto(e));
    }
}
