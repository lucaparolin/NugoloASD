using System.Data;
using System.Data.SqlClient;
using AISSURE.Pilot.Data;
using AISSURE.Pilot.Models.Entities;
using AISSURE.Pilot.Repositories.Interfaces;
using AISSURE.Pilot.Utilities;

namespace AISSURE.Pilot.Repositories.Implementations;

/// <summary>
/// Repository per Associazioni usando ADO.NET
/// </summary>
public class AssociazioneRepository : BaseRepository<Associazione>, IAssociazioneRepository
{
    protected override string TableName => "Associazioni";

    public AssociazioneRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override async Task<IEnumerable<Associazione>> GetAllAsync(int tenantId)
    {
        var associazioni = new List<Associazione>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Associazioni WHERE AssociazioneId = @TenantId AND Attivo = 1";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            associazioni.Add(MapFromReader(reader));
        }

        return associazioni;
    }

    public override async Task<Associazione?> GetByIdAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Associazioni WHERE AssociazioneId = @Id AND AssociazioneId = @TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<Associazione?> GetByEmailAsync(string email)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Associazioni WHERE Email = @Email";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Email", email, SqlDbType.NVarChar, 100));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<Associazione?> GetByCodiceFiscaleAsync(string codiceFiscale)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Associazioni WHERE CodiceFiscale = @CodiceFiscale";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CodiceFiscale", codiceFiscale, SqlDbType.NVarChar, 16));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<Associazione?> GetByDominioAsync(string dominio)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Associazioni WHERE DominioPersonalizzato = @Dominio";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Dominio", dominio, SqlDbType.NVarChar, 100));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<bool> IsActiveAsync(int associazioneId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Attivo FROM Associazioni WHERE AssociazioneId = @Id";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", associazioneId, SqlDbType.Int));

        var result = await command.ExecuteScalarAsync();
        return result != null && (bool)result;
    }

    public override async Task<int> InsertAsync(Associazione entity, int tenantId)
    {
        SetInsertAuditFields(entity, "SYSTEM");

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Associazioni (
                Nome, RagioneSociale, PartitaIVA, CodiceFiscale,
                Indirizzo, Citta, CAP, Provincia, Regione, Nazione,
                Telefono, Email, PEC, SitoWeb,
                Logo, ColoriPrimari, DominioPersonalizzato,
                TipoSport, NumeroMassimoSoci, DataScadenzaAbbonamento, PianoAbbonamento,
                Attivo, DataRegistrazione,
                DataInserimento, UtenteInserimento
            ) VALUES (
                @Nome, @RagioneSociale, @PartitaIVA, @CodiceFiscale,
                @Indirizzo, @Citta, @CAP, @Provincia, @Regione, @Nazione,
                @Telefono, @Email, @PEC, @SitoWeb,
                @Logo, @ColoriPrimari, @DominioPersonalizzato,
                @TipoSport, @NumeroMassimoSoci, @DataScadenzaAbbonamento, @PianoAbbonamento,
                @Attivo, @DataRegistrazione,
                @DataInserimento, @UtenteInserimento
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        AddParametersForInsert(command, entity);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public override async Task<bool> UpdateAsync(Associazione entity, int tenantId)
    {
        SetUpdateAuditFields(entity, "SYSTEM");

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Associazioni SET
                Nome = @Nome,
                RagioneSociale = @RagioneSociale,
                PartitaIVA = @PartitaIVA,
                Indirizzo = @Indirizzo,
                Citta = @Citta,
                CAP = @CAP,
                Provincia = @Provincia,
                Telefono = @Telefono,
                Email = @Email,
                PEC = @PEC,
                SitoWeb = @SitoWeb,
                Logo = @Logo,
                ColoriPrimari = @ColoriPrimari,
                DominioPersonalizzato = @DominioPersonalizzato,
                TipoSport = @TipoSport,
                Attivo = @Attivo,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE AssociazioneId = @Id";

        AddParametersForUpdate(command, entity);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public override async Task<bool> DeleteAsync(int id, int tenantId)
    {
        // Soft delete
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Associazioni SET
                Attivo = 0,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE AssociazioneId = @Id";

        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteModifica", "SYSTEM", SqlDbType.NVarChar, 100));

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public override async Task<bool> ExistsAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(1) FROM Associazioni WHERE AssociazioneId = @Id";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    private Associazione MapFromReader(IDataReader reader)
    {
        return new Associazione
        {
            AssociazioneId = reader.GetInt32(reader.GetOrdinal("AssociazioneId")),
            Nome = reader.GetString(reader.GetOrdinal("Nome")),
            RagioneSociale = reader.GetString(reader.GetOrdinal("RagioneSociale")),
            PartitaIVA = SqlParameterHelper.GetStringValue(reader["PartitaIVA"]),
            CodiceFiscale = reader.GetString(reader.GetOrdinal("CodiceFiscale")),
            Indirizzo = SqlParameterHelper.GetStringValue(reader["Indirizzo"]),
            Citta = SqlParameterHelper.GetStringValue(reader["Citta"]),
            CAP = SqlParameterHelper.GetStringValue(reader["CAP"]),
            Provincia = SqlParameterHelper.GetStringValue(reader["Provincia"]),
            Regione = SqlParameterHelper.GetStringValue(reader["Regione"]),
            Nazione = reader.GetString(reader.GetOrdinal("Nazione")),
            Telefono = SqlParameterHelper.GetStringValue(reader["Telefono"]),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            PEC = SqlParameterHelper.GetStringValue(reader["PEC"]),
            SitoWeb = SqlParameterHelper.GetStringValue(reader["SitoWeb"]),
            Logo = SqlParameterHelper.GetStringValue(reader["Logo"]),
            ColoriPrimari = SqlParameterHelper.GetStringValue(reader["ColoriPrimari"]),
            DominioPersonalizzato = SqlParameterHelper.GetStringValue(reader["DominioPersonalizzato"]),
            TipoSport = SqlParameterHelper.GetStringValue(reader["TipoSport"]),
            NumeroMassimoSoci = reader.GetInt32(reader.GetOrdinal("NumeroMassimoSoci")),
            DataScadenzaAbbonamento = SqlParameterHelper.GetValue<DateTime?>(reader["DataScadenzaAbbonamento"]),
            PianoAbbonamento = reader.GetString(reader.GetOrdinal("PianoAbbonamento")),
            Attivo = reader.GetBoolean(reader.GetOrdinal("Attivo")),
            DataRegistrazione = reader.GetDateTime(reader.GetOrdinal("DataRegistrazione")),
            DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento")),
            DataModifica = SqlParameterHelper.GetValue<DateTime?>(reader["DataModifica"]),
            UtenteInserimento = reader.GetString(reader.GetOrdinal("UtenteInserimento")),
            UtenteModifica = SqlParameterHelper.GetStringValue(reader["UtenteModifica"])
        };
    }

    private void AddParametersForInsert(IDbCommand command, Associazione entity)
    {
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Nome", entity.Nome, SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@RagioneSociale", entity.RagioneSociale, SqlDbType.NVarChar, 300));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@PartitaIVA", entity.PartitaIVA, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CodiceFiscale", entity.CodiceFiscale, SqlDbType.NVarChar, 16));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Indirizzo", entity.Indirizzo, SqlDbType.NVarChar, 300));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Citta", entity.Citta, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CAP", entity.CAP, SqlDbType.NVarChar, 10));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Provincia", entity.Provincia, SqlDbType.NVarChar, 2));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Regione", entity.Regione, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Nazione", entity.Nazione, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Telefono", entity.Telefono, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Email", entity.Email, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@PEC", entity.PEC, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@SitoWeb", entity.SitoWeb, SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Logo", entity.Logo, SqlDbType.NVarChar, 500));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@ColoriPrimari", entity.ColoriPrimari, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DominioPersonalizzato", entity.DominioPersonalizzato, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TipoSport", entity.TipoSport, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@NumeroMassimoSoci", entity.NumeroMassimoSoci, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataScadenzaAbbonamento", entity.DataScadenzaAbbonamento, SqlDbType.Date));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@PianoAbbonamento", entity.PianoAbbonamento, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Attivo", entity.Attivo, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataRegistrazione", entity.DataRegistrazione, SqlDbType.Date));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataInserimento", entity.DataInserimento, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteInserimento", entity.UtenteInserimento, SqlDbType.NVarChar, 100));
    }

    private void AddParametersForUpdate(IDbCommand command, Associazione entity)
    {
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", entity.AssociazioneId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Nome", entity.Nome, SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@RagioneSociale", entity.RagioneSociale, SqlDbType.NVarChar, 300));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@PartitaIVA", entity.PartitaIVA, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Indirizzo", entity.Indirizzo, SqlDbType.NVarChar, 300));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Citta", entity.Citta, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CAP", entity.CAP, SqlDbType.NVarChar, 10));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Provincia", entity.Provincia, SqlDbType.NVarChar, 2));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Telefono", entity.Telefono, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Email", entity.Email, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@PEC", entity.PEC, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@SitoWeb", entity.SitoWeb, SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Logo", entity.Logo, SqlDbType.NVarChar, 500));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@ColoriPrimari", entity.ColoriPrimari, SqlDbType.NVarChar, 50));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DominioPersonalizzato", entity.DominioPersonalizzato, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TipoSport", entity.TipoSport, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Attivo", entity.Attivo, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", entity.DataModifica, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteModifica", entity.UtenteModifica, SqlDbType.NVarChar, 100));
    }
}
