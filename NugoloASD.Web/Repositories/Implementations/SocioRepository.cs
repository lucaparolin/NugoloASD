using System.Data;
using System.Data.SqlClient;
using NugoloASD.Web.Data;
using NugoloASD.Web.Models.Entities;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Utilities;

namespace NugoloASD.Web.Repositories.Implementations;

public class SocioRepository : BaseRepository<Socio>, ISocioRepository
{
    protected override string TableName => "Soci";

    public SocioRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory) { }

    public override async Task<IEnumerable<Socio>> GetAllAsync(int tenantId)
    {
        var soci = new List<Socio>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Soci WHERE AssociazioneId = @TenantId ORDER BY Cognome, Nome";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) soci.Add(MapFromReader(reader));
        return soci;
    }

    public override async Task<Socio?> GetByIdAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Soci WHERE SocioId = @Id AND AssociazioneId = @TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync()) return MapFromReader(reader);
        return null;
    }

    public async Task<Socio?> GetByCodiceFiscaleAsync(string codiceFiscale, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Soci WHERE CodiceFiscale = @CF AND AssociazioneId = @TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CF", codiceFiscale, SqlDbType.NVarChar, 16));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync()) return MapFromReader(reader);
        return null;
    }

    public async Task<Socio?> GetByNumeroTesseraAsync(string numeroTessera, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Soci WHERE NumeroTessera = @NT AND AssociazioneId = @TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@NT", numeroTessera, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync()) return MapFromReader(reader);
        return null;
    }

    public async Task<IEnumerable<Socio>> GetByTipoAsync(string tipoSocio, int tenantId)
    {
        var soci = new List<Socio>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Soci WHERE TipoSocio = @Tipo AND AssociazioneId = @TenantId ORDER BY Cognome, Nome";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Tipo", tipoSocio, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) soci.Add(MapFromReader(reader));
        return soci;
    }

    public async Task<IEnumerable<Socio>> GetByStatoAsync(string statoSocio, int tenantId)
    {
        var soci = new List<Socio>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Soci WHERE StatoSocio = @Stato AND AssociazioneId = @TenantId ORDER BY Cognome, Nome";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Stato", statoSocio, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) soci.Add(MapFromReader(reader));
        return soci;
    }

    public async Task<IEnumerable<Socio>> GetSociConCertificatoInScadenzaAsync(int giorniPrimaScadenza, int tenantId)
    {
        var soci = new List<Socio>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT DISTINCT S.* FROM Soci S
            INNER JOIN CertificatiMedici CM ON S.SocioId = CM.SocioId
            WHERE S.AssociazioneId = @TenantId
            AND CM.DataScadenza <= DATEADD(day, @Giorni, GETDATE())
            AND CM.DataScadenza >= GETDATE() AND CM.Validato = 1
            ORDER BY CM.DataScadenza, S.Cognome, S.Nome";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Giorni", giorniPrimaScadenza, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) soci.Add(MapFromReader(reader));
        return soci;
    }

    public async Task<IEnumerable<Socio>> SearchAsync(string searchTerm, int tenantId)
    {
        var soci = new List<Socio>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Soci WHERE AssociazioneId = @TenantId
            AND (Nome LIKE @ST OR Cognome LIKE @ST OR CodiceFiscale LIKE @ST OR Email LIKE @ST OR NumeroTessera LIKE @ST)
            ORDER BY Cognome, Nome";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@ST", $"%{searchTerm}%", SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) soci.Add(MapFromReader(reader));
        return soci;
    }

    public override async Task<int> InsertAsync(Socio entity, int tenantId)
    {
        SetInsertAuditFields(entity, entity.UtenteInserimento);
        entity.AssociazioneId = tenantId;
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Soci (AssociazioneId,UtenteId,Nome,Cognome,CodiceFiscale,DataNascita,LuogoNascita,Sesso,
            TipoDocumento,NumeroDocumento,DataRilascioDocumento,DataScadenzaDocumento,EnteRilascio,
            IndirizzoResidenza,CittaResidenza,CAPResidenza,ProvinciaResidenza,
            IndirizzoDomicilio,CittaDomicilio,CAPDomicilio,ProvinciaDomicilio,
            Email,Telefono,TelefonoCellulare,NumeroTessera,TipoSocio,DataPrimaIscrizione,DataUltimoRinnovo,StatoSocio,
            Minorenne,GenitoreId,NomeGenitore1,CognomeGenitore1,TelefonoGenitore1,EmailGenitore1,
            NomeGenitore2,CognomeGenitore2,TelefonoGenitore2,EmailGenitore2,
            ConsensoPrivacy,DataConsensoPrivacy,ConsensoMarketing,ConsensoImmagini,FotoUrl,Note,
            DataInserimento,UtenteInserimento)
            VALUES (@AssociazioneId,@UtenteId,@Nome,@Cognome,@CodiceFiscale,@DataNascita,@LuogoNascita,@Sesso,
            @TipoDocumento,@NumeroDocumento,@DataRilascioDocumento,@DataScadenzaDocumento,@EnteRilascio,
            @IndirizzoResidenza,@CittaResidenza,@CAPResidenza,@ProvinciaResidenza,
            @IndirizzoDomicilio,@CittaDomicilio,@CAPDomicilio,@ProvinciaDomicilio,
            @Email,@Telefono,@TelefonoCellulare,@NumeroTessera,@TipoSocio,@DataPrimaIscrizione,@DataUltimoRinnovo,@StatoSocio,
            @Minorenne,@GenitoreId,@NomeGenitore1,@CognomeGenitore1,@TelefonoGenitore1,@EmailGenitore1,
            @NomeGenitore2,@CognomeGenitore2,@TelefonoGenitore2,@EmailGenitore2,
            @ConsensoPrivacy,@DataConsensoPrivacy,@ConsensoMarketing,@ConsensoImmagini,@FotoUrl,@Note,
            @DataInserimento,@UtenteInserimento);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
        AddInsertParams(command, entity);
        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public override async Task<bool> UpdateAsync(Socio entity, int tenantId)
    {
        SetUpdateAuditFields(entity, entity.UtenteModifica ?? entity.UtenteInserimento);
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Soci SET Nome=@Nome,Cognome=@Cognome,CodiceFiscale=@CodiceFiscale,DataNascita=@DataNascita,
            IndirizzoResidenza=@IndirizzoResidenza,CittaResidenza=@CittaResidenza,Email=@Email,Telefono=@Telefono,
            TelefonoCellulare=@TelefonoCellulare,StatoSocio=@StatoSocio,Note=@Note,
            DataModifica=@DataModifica,UtenteModifica=@UtenteModifica
            WHERE SocioId=@Id AND AssociazioneId=@TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", entity.SocioId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Nome", entity.Nome, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Cognome", entity.Cognome, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CodiceFiscale", entity.CodiceFiscale, SqlDbType.NVarChar, 16));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataNascita", entity.DataNascita, SqlDbType.Date));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@IndirizzoResidenza", entity.IndirizzoResidenza, SqlDbType.NVarChar, 300));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CittaResidenza", entity.CittaResidenza, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Email", entity.Email, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Telefono", entity.Telefono, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TelefonoCellulare", entity.TelefonoCellulare, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@StatoSocio", entity.StatoSocio, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Note", entity.Note, SqlDbType.NVarChar, -1));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", entity.DataModifica, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteModifica", entity.UtenteModifica, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        return await command.ExecuteNonQueryAsync() > 0;
    }

    public override async Task<bool> DeleteAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Soci SET StatoSocio='Storico',DataModifica=@DM,UtenteModifica='SYSTEM' WHERE SocioId=@Id AND AssociazioneId=@TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DM", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        return await command.ExecuteNonQueryAsync() > 0;
    }

    public override async Task<bool> ExistsAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(1) FROM Soci WHERE SocioId=@Id AND AssociazioneId=@TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
        return Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
    }

    private Socio MapFromReader(IDataReader r) => new Socio
    {
        SocioId = r.GetInt32(r.GetOrdinal("SocioId")),
        AssociazioneId = r.GetInt32(r.GetOrdinal("AssociazioneId")),
        UtenteId = SqlParameterHelper.GetValue<int?>(r["UtenteId"]),
        Nome = r.GetString(r.GetOrdinal("Nome")),
        Cognome = r.GetString(r.GetOrdinal("Cognome")),
        CodiceFiscale = r.GetString(r.GetOrdinal("CodiceFiscale")),
        DataNascita = r.GetDateTime(r.GetOrdinal("DataNascita")),
        LuogoNascita = SqlParameterHelper.GetStringValue(r["LuogoNascita"]),
        Sesso = SqlParameterHelper.GetStringValue(r["Sesso"]),
        TipoDocumento = SqlParameterHelper.GetStringValue(r["TipoDocumento"]),
        NumeroDocumento = SqlParameterHelper.GetStringValue(r["NumeroDocumento"]),
        DataRilascioDocumento = SqlParameterHelper.GetValue<DateTime?>(r["DataRilascioDocumento"]),
        DataScadenzaDocumento = SqlParameterHelper.GetValue<DateTime?>(r["DataScadenzaDocumento"]),
        EnteRilascio = SqlParameterHelper.GetStringValue(r["EnteRilascio"]),
        IndirizzoResidenza = SqlParameterHelper.GetStringValue(r["IndirizzoResidenza"]),
        CittaResidenza = SqlParameterHelper.GetStringValue(r["CittaResidenza"]),
        CAPResidenza = SqlParameterHelper.GetStringValue(r["CAPResidenza"]),
        ProvinciaResidenza = SqlParameterHelper.GetStringValue(r["ProvinciaResidenza"]),
        IndirizzoDomicilio = SqlParameterHelper.GetStringValue(r["IndirizzoDomicilio"]),
        CittaDomicilio = SqlParameterHelper.GetStringValue(r["CittaDomicilio"]),
        CAPDomicilio = SqlParameterHelper.GetStringValue(r["CAPDomicilio"]),
        ProvinciaDomicilio = SqlParameterHelper.GetStringValue(r["ProvinciaDomicilio"]),
        Email = SqlParameterHelper.GetStringValue(r["Email"]),
        Telefono = SqlParameterHelper.GetStringValue(r["Telefono"]),
        TelefonoCellulare = SqlParameterHelper.GetStringValue(r["TelefonoCellulare"]),
        NumeroTessera = SqlParameterHelper.GetStringValue(r["NumeroTessera"]),
        TipoSocio = r.GetString(r.GetOrdinal("TipoSocio")),
        DataPrimaIscrizione = SqlParameterHelper.GetValue<DateTime?>(r["DataPrimaIscrizione"]),
        DataUltimoRinnovo = SqlParameterHelper.GetValue<DateTime?>(r["DataUltimoRinnovo"]),
        StatoSocio = r.GetString(r.GetOrdinal("StatoSocio")),
        Minorenne = r.GetBoolean(r.GetOrdinal("Minorenne")),
        GenitoreId = SqlParameterHelper.GetValue<int?>(r["GenitoreId"]),
        NomeGenitore1 = SqlParameterHelper.GetStringValue(r["NomeGenitore1"]),
        CognomeGenitore1 = SqlParameterHelper.GetStringValue(r["CognomeGenitore1"]),
        TelefonoGenitore1 = SqlParameterHelper.GetStringValue(r["TelefonoGenitore1"]),
        EmailGenitore1 = SqlParameterHelper.GetStringValue(r["EmailGenitore1"]),
        NomeGenitore2 = SqlParameterHelper.GetStringValue(r["NomeGenitore2"]),
        CognomeGenitore2 = SqlParameterHelper.GetStringValue(r["CognomeGenitore2"]),
        TelefonoGenitore2 = SqlParameterHelper.GetStringValue(r["TelefonoGenitore2"]),
        EmailGenitore2 = SqlParameterHelper.GetStringValue(r["EmailGenitore2"]),
        ConsensoPrivacy = r.GetBoolean(r.GetOrdinal("ConsensoPrivacy")),
        DataConsensoPrivacy = SqlParameterHelper.GetValue<DateTime?>(r["DataConsensoPrivacy"]),
        ConsensoMarketing = r.GetBoolean(r.GetOrdinal("ConsensoMarketing")),
        ConsensoImmagini = r.GetBoolean(r.GetOrdinal("ConsensoImmagini")),
        FotoUrl = SqlParameterHelper.GetStringValue(r["FotoUrl"]),
        Note = SqlParameterHelper.GetStringValue(r["Note"]),
        DataInserimento = r.GetDateTime(r.GetOrdinal("DataInserimento")),
        DataModifica = SqlParameterHelper.GetValue<DateTime?>(r["DataModifica"]),
        UtenteInserimento = r.GetString(r.GetOrdinal("UtenteInserimento")),
        UtenteModifica = SqlParameterHelper.GetStringValue(r["UtenteModifica"])
    };

    private void AddInsertParams(IDbCommand cmd, Socio e)
    {
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@AssociazioneId", e.AssociazioneId, SqlDbType.Int));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteId", e.UtenteId, SqlDbType.Int));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@Nome", e.Nome, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@Cognome", e.Cognome, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@CodiceFiscale", e.CodiceFiscale, SqlDbType.NVarChar, 16));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@DataNascita", e.DataNascita, SqlDbType.Date));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@LuogoNascita", e.LuogoNascita, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@Sesso", e.Sesso, SqlDbType.Char, 1));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@TipoDocumento", e.TipoDocumento, SqlDbType.NVarChar, 50));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@NumeroDocumento", e.NumeroDocumento, SqlDbType.NVarChar, 50));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@DataRilascioDocumento", e.DataRilascioDocumento, SqlDbType.Date));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@DataScadenzaDocumento", e.DataScadenzaDocumento, SqlDbType.Date));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@EnteRilascio", e.EnteRilascio, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@IndirizzoResidenza", e.IndirizzoResidenza, SqlDbType.NVarChar, 300));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@CittaResidenza", e.CittaResidenza, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@CAPResidenza", e.CAPResidenza, SqlDbType.NVarChar, 10));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@ProvinciaResidenza", e.ProvinciaResidenza, SqlDbType.NVarChar, 2));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@IndirizzoDomicilio", e.IndirizzoDomicilio, SqlDbType.NVarChar, 300));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@CittaDomicilio", e.CittaDomicilio, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@CAPDomicilio", e.CAPDomicilio, SqlDbType.NVarChar, 10));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@ProvinciaDomicilio", e.ProvinciaDomicilio, SqlDbType.NVarChar, 2));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@Email", e.Email, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@Telefono", e.Telefono, SqlDbType.NVarChar, 20));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@TelefonoCellulare", e.TelefonoCellulare, SqlDbType.NVarChar, 20));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@NumeroTessera", e.NumeroTessera, SqlDbType.NVarChar, 50));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@TipoSocio", e.TipoSocio, SqlDbType.NVarChar, 50));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@DataPrimaIscrizione", e.DataPrimaIscrizione, SqlDbType.Date));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@DataUltimoRinnovo", e.DataUltimoRinnovo, SqlDbType.Date));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@StatoSocio", e.StatoSocio, SqlDbType.NVarChar, 50));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@Minorenne", e.Minorenne, SqlDbType.Bit));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@GenitoreId", e.GenitoreId, SqlDbType.Int));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@NomeGenitore1", e.NomeGenitore1, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@CognomeGenitore1", e.CognomeGenitore1, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@TelefonoGenitore1", e.TelefonoGenitore1, SqlDbType.NVarChar, 20));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@EmailGenitore1", e.EmailGenitore1, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@NomeGenitore2", e.NomeGenitore2, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@CognomeGenitore2", e.CognomeGenitore2, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@TelefonoGenitore2", e.TelefonoGenitore2, SqlDbType.NVarChar, 20));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@EmailGenitore2", e.EmailGenitore2, SqlDbType.NVarChar, 100));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@ConsensoPrivacy", e.ConsensoPrivacy, SqlDbType.Bit));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@DataConsensoPrivacy", e.DataConsensoPrivacy, SqlDbType.DateTime));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@ConsensoMarketing", e.ConsensoMarketing, SqlDbType.Bit));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@ConsensoImmagini", e.ConsensoImmagini, SqlDbType.Bit));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@FotoUrl", e.FotoUrl, SqlDbType.NVarChar, 500));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@Note", e.Note, SqlDbType.NVarChar, -1));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@DataInserimento", e.DataInserimento, SqlDbType.DateTime));
        cmd.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteInserimento", e.UtenteInserimento, SqlDbType.NVarChar, 100));
    }
}
