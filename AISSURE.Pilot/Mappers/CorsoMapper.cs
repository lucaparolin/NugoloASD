using AISSURE.Pilot.DTOs.Corsi;
using AISSURE.Pilot.Models.Entities;

namespace AISSURE.Pilot.Mappers;

/// <summary>
/// Mapper per conversione Entity Corso <-> DTO
/// </summary>
public static class CorsoMapper
{
    /// <summary>
    /// Converte Entity Corso in CorsoListDto
    /// </summary>
    public static CorsoListDto ToListDto(Corso entity)
    {
        return new CorsoListDto
        {
            CorsoId = entity.CorsoId,
            Nome = entity.Nome,
            Categoria = entity.Categoria,
            Livello = entity.Livello,
            DataInizio = entity.DataInizio,
            DataFine = entity.DataFine,
            AnnoSportivo = entity.AnnoSportivo,
            PostiDisponibili = entity.PostiDisponibili,
            PostiOccupati = entity.PostiOccupati,
            ListaAttesaAttiva = entity.ListaAttesaAttiva,
            PrezzoPieno = entity.PrezzoPieno,
            Attivo = entity.Attivo,
            PubblicatoOnline = entity.PubblicatoOnline
        };
    }

    /// <summary>
    /// Converte Entity Corso in CorsoDetailDto
    /// </summary>
    public static CorsoDetailDto ToDetailDto(Corso entity)
    {
        return new CorsoDetailDto
        {
            CorsoId = entity.CorsoId,
            Nome = entity.Nome,
            Descrizione = entity.Descrizione,
            Categoria = entity.Categoria,
            Livello = entity.Livello,
            DataInizio = entity.DataInizio,
            DataFine = entity.DataFine,
            AnnoSportivo = entity.AnnoSportivo,
            GiorniSettimana = entity.GiorniSettimana,
            OrarioInizio = entity.OrarioInizio,
            OrarioFine = entity.OrarioFine,
            PostiDisponibili = entity.PostiDisponibili,
            PostiOccupati = entity.PostiOccupati,
            ListaAttesaAttiva = entity.ListaAttesaAttiva,
            IstruttoreId = entity.IstruttoreId,
            NomeIstruttore = entity.Istruttore?.Nome,
            CognomeIstruttore = entity.Istruttore?.Cognome,
            PrezzoPieno = entity.PrezzoPieno,
            PrezzoRidotto = entity.PrezzoRidotto,
            DescrizioneRiduzione = entity.DescrizioneRiduzione,
            ImmagineUrl = entity.ImmagineUrl,
            Attivo = entity.Attivo,
            PubblicatoOnline = entity.PubblicatoOnline,
            DataInserimento = entity.DataInserimento,
            DataModifica = entity.DataModifica
        };
    }

    /// <summary>
    /// Converte CorsoCreateDto in Entity Corso
    /// </summary>
    public static Corso ToEntity(CorsoCreateDto dto)
    {
        return new Corso
        {
            Nome = dto.Nome,
            Descrizione = dto.Descrizione,
            Categoria = dto.Categoria,
            Livello = dto.Livello,
            DataInizio = dto.DataInizio,
            DataFine = dto.DataFine,
            AnnoSportivo = dto.AnnoSportivo,
            GiorniSettimana = dto.GiorniSettimana,
            OrarioInizio = dto.OrarioInizio,
            OrarioFine = dto.OrarioFine,
            PostiDisponibili = dto.PostiDisponibili,
            PostiOccupati = 0, // Inizializzato a 0
            ListaAttesaAttiva = dto.ListaAttesaAttiva,
            IstruttoreId = dto.IstruttoreId,
            PrezzoPieno = dto.PrezzoPieno,
            PrezzoRidotto = dto.PrezzoRidotto,
            DescrizioneRiduzione = dto.DescrizioneRiduzione,
            Attivo = dto.Attivo,
            PubblicatoOnline = dto.PubblicatoOnline
        };
    }

    /// <summary>
    /// Aggiorna Entity Corso con i dati da CorsoUpdateDto
    /// </summary>
    public static void UpdateEntity(Corso entity, CorsoUpdateDto dto)
    {
        entity.Nome = dto.Nome;
        entity.Descrizione = dto.Descrizione;
        entity.Categoria = dto.Categoria;
        entity.Livello = dto.Livello;
        entity.DataInizio = dto.DataInizio;
        entity.DataFine = dto.DataFine;
        entity.AnnoSportivo = dto.AnnoSportivo;
        entity.GiorniSettimana = dto.GiorniSettimana;
        entity.OrarioInizio = dto.OrarioInizio;
        entity.OrarioFine = dto.OrarioFine;
        entity.PostiDisponibili = dto.PostiDisponibili;
        entity.ListaAttesaAttiva = dto.ListaAttesaAttiva;
        entity.IstruttoreId = dto.IstruttoreId;
        entity.PrezzoPieno = dto.PrezzoPieno;
        entity.PrezzoRidotto = dto.PrezzoRidotto;
        entity.DescrizioneRiduzione = dto.DescrizioneRiduzione;
        entity.Attivo = dto.Attivo;
        entity.PubblicatoOnline = dto.PubblicatoOnline;
    }

    /// <summary>
    /// Converte lista di Entity in lista di ListDto
    /// </summary>
    public static IEnumerable<CorsoListDto> ToListDto(IEnumerable<Corso> entities)
    {
        return entities.Select(ToListDto);
    }
}
