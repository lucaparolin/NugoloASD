using AISSURE.Pilot.DTOs.Soci;
using AISSURE.Pilot.Models.Entities;

namespace AISSURE.Pilot.Mappers;

/// <summary>
/// Mapper per conversione Entity Socio <-> DTO
/// </summary>
public static class SocioMapper
{
    /// <summary>
    /// Converte Entity Socio in SocioListDto
    /// </summary>
    public static SocioListDto ToListDto(Socio entity)
    {
        return new SocioListDto
        {
            SocioId = entity.SocioId,
            Nome = entity.Nome,
            Cognome = entity.Cognome,
            CodiceFiscale = entity.CodiceFiscale,
            DataNascita = entity.DataNascita,
            Email = entity.Email,
            Telefono = entity.Telefono,
            NumeroTessera = entity.NumeroTessera,
            TipoSocio = entity.TipoSocio,
            StatoSocio = entity.StatoSocio,
            CertificatoMedicoValido = entity.CertificatoMedicoValido,
            DataScadenzaCertificato = entity.DataScadenzaCertificato
        };
    }

    /// <summary>
    /// Converte Entity Socio in SocioDetailDto
    /// </summary>
    public static SocioDetailDto ToDetailDto(Socio entity)
    {
        return new SocioDetailDto
        {
            SocioId = entity.SocioId,
            Nome = entity.Nome,
            Cognome = entity.Cognome,
            CodiceFiscale = entity.CodiceFiscale,
            DataNascita = entity.DataNascita,
            LuogoNascita = entity.LuogoNascita,
            ProvinciaNascita = entity.ProvinciaNascita,
            NazioneNascita = entity.NazioneNascita,
            Sesso = entity.Sesso,
            Email = entity.Email,
            Telefono = entity.Telefono,
            Cellulare = entity.Cellulare,
            Indirizzo = entity.Indirizzo,
            Citta = entity.Citta,
            Provincia = entity.Provincia,
            CAP = entity.CAP,
            Nazione = entity.Nazione,
            NumeroTessera = entity.NumeroTessera,
            DataIscrizione = entity.DataIscrizione,
            TipoSocio = entity.TipoSocio,
            StatoSocio = entity.StatoSocio,
            Minorenne = entity.Minorenne,
            GenitoreId = entity.GenitoreId,
            NomeGenitore = entity.NomeGenitore,
            CognomeGenitore = entity.CognomeGenitore,
            NumeroDocumento = entity.NumeroDocumento,
            TipoDocumento = entity.TipoDocumento,
            DataScadenzaDocumento = entity.DataScadenzaDocumento,
            CertificatoMedicoValido = entity.CertificatoMedicoValido,
            DataCertificatoMedico = entity.DataCertificatoMedico,
            DataScadenzaCertificato = entity.DataScadenzaCertificato,
            ConsensoPrivacy = entity.ConsensoPrivacy,
            DataConsensoPrivacy = entity.DataConsensoPrivacy,
            ConsensoMarketing = entity.ConsensoMarketing,
            ConsensoImmagini = entity.ConsensoImmagini,
            FotoUrl = entity.FotoUrl,
            Note = entity.Note,
            DataInserimento = entity.DataInserimento,
            DataModifica = entity.DataModifica
        };
    }

    /// <summary>
    /// Converte SocioCreateDto in Entity Socio
    /// </summary>
    public static Socio ToEntity(SocioCreateDto dto)
    {
        return new Socio
        {
            Nome = dto.Nome,
            Cognome = dto.Cognome,
            CodiceFiscale = dto.CodiceFiscale,
            DataNascita = dto.DataNascita,
            LuogoNascita = dto.LuogoNascita,
            ProvinciaNascita = dto.ProvinciaNascita,
            NazioneNascita = dto.NazioneNascita,
            Sesso = dto.Sesso,
            Email = dto.Email,
            Telefono = dto.Telefono,
            Cellulare = dto.Cellulare,
            Indirizzo = dto.Indirizzo,
            Citta = dto.Citta,
            Provincia = dto.Provincia,
            CAP = dto.CAP,
            Nazione = dto.Nazione,
            NumeroTessera = dto.NumeroTessera,
            DataIscrizione = dto.DataIscrizione ?? DateTime.Today,
            TipoSocio = dto.TipoSocio,
            StatoSocio = dto.StatoSocio,
            Minorenne = dto.Minorenne,
            GenitoreId = dto.GenitoreId,
            NomeGenitore = dto.NomeGenitore,
            CognomeGenitore = dto.CognomeGenitore,
            EmailGenitore = dto.EmailGenitore,
            TelefonoGenitore = dto.TelefonoGenitore,
            NumeroDocumento = dto.NumeroDocumento,
            TipoDocumento = dto.TipoDocumento,
            DataScadenzaDocumento = dto.DataScadenzaDocumento,
            CertificatoMedicoValido = dto.CertificatoMedicoValido,
            DataCertificatoMedico = dto.DataCertificatoMedico,
            DataScadenzaCertificato = dto.DataScadenzaCertificato,
            ConsensoPrivacy = dto.ConsensoPrivacy,
            DataConsensoPrivacy = dto.ConsensoPrivacy ? DateTime.UtcNow : (DateTime?)null,
            ConsensoMarketing = dto.ConsensoMarketing,
            ConsensoImmagini = dto.ConsensoImmagini,
            Note = dto.Note
        };
    }

    /// <summary>
    /// Aggiorna Entity Socio con i dati da SocioUpdateDto
    /// </summary>
    public static void UpdateEntity(Socio entity, SocioUpdateDto dto)
    {
        entity.Nome = dto.Nome;
        entity.Cognome = dto.Cognome;
        entity.CodiceFiscale = dto.CodiceFiscale;
        entity.DataNascita = dto.DataNascita;
        entity.LuogoNascita = dto.LuogoNascita;
        entity.ProvinciaNascita = dto.ProvinciaNascita;
        entity.NazioneNascita = dto.NazioneNascita;
        entity.Sesso = dto.Sesso;
        entity.Email = dto.Email;
        entity.Telefono = dto.Telefono;
        entity.Cellulare = dto.Cellulare;
        entity.Indirizzo = dto.Indirizzo;
        entity.Citta = dto.Citta;
        entity.Provincia = dto.Provincia;
        entity.CAP = dto.CAP;
        entity.Nazione = dto.Nazione;
        entity.NumeroTessera = dto.NumeroTessera;
        entity.TipoSocio = dto.TipoSocio;
        entity.StatoSocio = dto.StatoSocio;
        entity.Minorenne = dto.Minorenne;
        entity.GenitoreId = dto.GenitoreId;
        entity.NomeGenitore = dto.NomeGenitore;
        entity.CognomeGenitore = dto.CognomeGenitore;
        entity.EmailGenitore = dto.EmailGenitore;
        entity.TelefonoGenitore = dto.TelefonoGenitore;
        entity.NumeroDocumento = dto.NumeroDocumento;
        entity.TipoDocumento = dto.TipoDocumento;
        entity.DataScadenzaDocumento = dto.DataScadenzaDocumento;
        entity.CertificatoMedicoValido = dto.CertificatoMedicoValido;
        entity.DataCertificatoMedico = dto.DataCertificatoMedico;
        entity.DataScadenzaCertificato = dto.DataScadenzaCertificato;
        entity.ConsensoPrivacy = dto.ConsensoPrivacy;
        entity.ConsensoMarketing = dto.ConsensoMarketing;
        entity.ConsensoImmagini = dto.ConsensoImmagini;
        entity.Note = dto.Note;
    }

    /// <summary>
    /// Converte lista di Entity in lista di ListDto
    /// </summary>
    public static IEnumerable<SocioListDto> ToListDto(IEnumerable<Socio> entities)
    {
        return entities.Select(ToListDto);
    }
}
