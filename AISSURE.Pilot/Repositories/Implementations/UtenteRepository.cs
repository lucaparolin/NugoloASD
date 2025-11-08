using System.Data;
using System.Data.SqlClient;
using AISSURE.Pilot.Data;
using AISSURE.Pilot.Models.Entities;
using AISSURE.Pilot.Repositories.Interfaces;
using AISSURE.Pilot.Utilities;

namespace AISSURE.Pilot.Repositories.Implementations;

/// <summary>
/// Repository per Utenti usando ADO.NET
/// </summary>
public class UtenteRepository : BaseRepository<Utente>, IUtenteRepository
{
    protected override string TableName => "Utenti";

    public UtenteRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override async Task<IEnumerable<Utente>> GetAllAsync(int tenantId)
    {
        var utenti = new List<Utente>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Utenti WHERE AssociazioneId = @TenantId AND Attivo = 1";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            utenti.Add(MapFromReader(reader));
        }

        return utenti;
    }

    public override async Task<Utente?> GetByIdAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Utenti WHERE UtenteId = @Id AND AssociazioneId = @TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<Utente?> GetByUsernameAsync(string username, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Utenti WHERE Username = @Username AND AssociazioneId = @TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Username", username, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<Utente?> GetByEmailAsync(string email, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Utenti WHERE Email = @Email AND AssociazioneId = @TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Email", email, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Ruolo>> GetUserRolesAsync(int utenteId, int tenantId)
    {
        var ruoli = new List<Ruolo>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT R.*
            FROM Ruoli R
            INNER JOIN UtentiRuoli UR ON R.RuoloId = UR.RuoloId
            WHERE UR.UtenteId = @UtenteId AND UR.AssociazioneId = @TenantId";

        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteId", utenteId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            ruoli.Add(new Ruolo
            {
                RuoloId = reader.GetInt32(reader.GetOrdinal("RuoloId")),
                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                Descrizione = SqlParameterHelper.GetStringValue(reader["Descrizione"]),
                Livello = reader.GetInt32(reader.GetOrdinal("Livello")),
                DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento")),
                DataModifica = SqlParameterHelper.GetValue<DateTime?>(reader["DataModifica"]),
                UtenteInserimento = reader.GetString(reader.GetOrdinal("UtenteInserimento")),
                UtenteModifica = SqlParameterHelper.GetStringValue(reader["UtenteModifica"])
            });
        }

        return ruoli;
    }

    public async Task<bool> UpdatePasswordAsync(int utenteId, string passwordHash, string salt, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Utenti SET
                PasswordHash = @PasswordHash,
                Salt = @Salt,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE UtenteId = @UtenteId AND AssociazioneId = @TenantId";

        command.Parameters.Add(SqlParameterHelper.CreateParameter("@PasswordHash", passwordHash, SqlDbType.NVarChar, 500));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Salt", salt, SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteModifica", $"User-{utenteId}", SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteId", utenteId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task IncrementFailedLoginAttemptsAsync(int utenteId, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Utenti SET
                TentativiAccessoFalliti = TentativiAccessoFalliti + 1,
                DataModifica = @DataModifica
            WHERE UtenteId = @UtenteId AND AssociazioneId = @TenantId";

        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteId", utenteId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        await command.ExecuteNonQueryAsync();
    }

    public async Task ResetFailedLoginAttemptsAsync(int utenteId, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Utenti SET
                TentativiAccessoFalliti = 0,
                UltimoAccesso = @UltimoAccesso,
                DataModifica = @DataModifica
            WHERE UtenteId = @UtenteId AND AssociazioneId = @TenantId";

        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UltimoAccesso", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteId", utenteId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        await command.ExecuteNonQueryAsync();
    }

    public async Task LockAccountAsync(int utenteId, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Utenti SET
                AccountBloccato = 1,
                DataBlocco = @DataBlocco,
                DataModifica = @DataModifica
            WHERE UtenteId = @UtenteId AND AssociazioneId = @TenantId";

        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataBlocco", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteId", utenteId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        await command.ExecuteNonQueryAsync();
    }

    public async Task UnlockAccountAsync(int utenteId, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Utenti SET
                AccountBloccato = 0,
                DataBlocco = NULL,
                TentativiAccessoFalliti = 0,
                DataModifica = @DataModifica
            WHERE UtenteId = @UtenteId AND AssociazioneId = @TenantId";

        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteId", utenteId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        await command.ExecuteNonQueryAsync();
    }

    public override async Task<int> InsertAsync(Utente entity, int tenantId)
    {
        SetInsertAuditFields(entity, entity.UtenteInserimento);
        entity.AssociazioneId = tenantId;

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Utenti (
                AssociazioneId, Username, Email, PasswordHash, Salt,
                Nome, Cognome, CodiceFiscale, DataNascita, LuogoNascita,
                Telefono, TelefonoCellulare,
                TwoFactorEnabled, TwoFactorSecret,
                TentativiAccessoFalliti, AccountBloccato,
                Attivo, EmailConfermata, TokenConfermaEmail,
                DataInserimento, UtenteInserimento
            ) VALUES (
                @AssociazioneId, @Username, @Email, @PasswordHash, @Salt,
                @Nome, @Cognome, @CodiceFiscale, @DataNascita, @LuogoNascita,
                @Telefono, @TelefonoCellulare,
                @TwoFactorEnabled, @TwoFactorSecret,
                @TentativiAccessoFalliti, @AccountBloccato,
                @Attivo, @EmailConfermata, @TokenConfermaEmail,
                @DataInserimento, @UtenteInserimento
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        AddParametersForInsert(command, entity);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public override async Task<bool> UpdateAsync(Utente entity, int tenantId)
    {
        SetUpdateAuditFields(entity, entity.UtenteModifica ?? entity.UtenteInserimento);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Utenti SET
                Username = @Username,
                Email = @Email,
                Nome = @Nome,
                Cognome = @Cognome,
                CodiceFiscale = @CodiceFiscale,
                DataNascita = @DataNascita,
                LuogoNascita = @LuogoNascita,
                Telefono = @Telefono,
                TelefonoCellulare = @TelefonoCellulare,
                TwoFactorEnabled = @TwoFactorEnabled,
                Attivo = @Attivo,
                EmailConfermata = @EmailConfermata,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE UtenteId = @Id AND AssociazioneId = @TenantId";

        AddParametersForUpdate(command, entity, tenantId);

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
            UPDATE Utenti SET
                Attivo = 0,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE UtenteId = @Id AND AssociazioneId = @TenantId";

        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", DateTime.UtcNow, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteModifica", "SYSTEM", SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public override async Task<bool> ExistsAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(1) FROM Utenti WHERE UtenteId = @Id AND AssociazioneId = @TenantId";
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", id, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }

    private Utente MapFromReader(IDataReader reader)
    {
        return new Utente
        {
            UtenteId = reader.GetInt32(reader.GetOrdinal("UtenteId")),
            AssociazioneId = reader.GetInt32(reader.GetOrdinal("AssociazioneId")),
            Username = reader.GetString(reader.GetOrdinal("Username")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
            Salt = reader.GetString(reader.GetOrdinal("Salt")),
            Nome = reader.GetString(reader.GetOrdinal("Nome")),
            Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
            CodiceFiscale = SqlParameterHelper.GetStringValue(reader["CodiceFiscale"]),
            DataNascita = SqlParameterHelper.GetValue<DateTime?>(reader["DataNascita"]),
            LuogoNascita = SqlParameterHelper.GetStringValue(reader["LuogoNascita"]),
            Telefono = SqlParameterHelper.GetStringValue(reader["Telefono"]),
            TelefonoCellulare = SqlParameterHelper.GetStringValue(reader["TelefonoCellulare"]),
            TwoFactorEnabled = reader.GetBoolean(reader.GetOrdinal("TwoFactorEnabled")),
            TwoFactorSecret = SqlParameterHelper.GetStringValue(reader["TwoFactorSecret"]),
            UltimoAccesso = SqlParameterHelper.GetValue<DateTime?>(reader["UltimoAccesso"]),
            TentativiAccessoFalliti = reader.GetInt32(reader.GetOrdinal("TentativiAccessoFalliti")),
            AccountBloccato = reader.GetBoolean(reader.GetOrdinal("AccountBloccato")),
            DataBlocco = SqlParameterHelper.GetValue<DateTime?>(reader["DataBlocco"]),
            Attivo = reader.GetBoolean(reader.GetOrdinal("Attivo")),
            EmailConfermata = reader.GetBoolean(reader.GetOrdinal("EmailConfermata")),
            TokenConfermaEmail = SqlParameterHelper.GetStringValue(reader["TokenConfermaEmail"]),
            TokenResetPassword = SqlParameterHelper.GetStringValue(reader["TokenResetPassword"]),
            DataScadenzaTokenReset = SqlParameterHelper.GetValue<DateTime?>(reader["DataScadenzaTokenReset"]),
            DataInserimento = reader.GetDateTime(reader.GetOrdinal("DataInserimento")),
            DataModifica = SqlParameterHelper.GetValue<DateTime?>(reader["DataModifica"]),
            UtenteInserimento = reader.GetString(reader.GetOrdinal("UtenteInserimento")),
            UtenteModifica = SqlParameterHelper.GetStringValue(reader["UtenteModifica"])
        };
    }

    private void AddParametersForInsert(IDbCommand command, Utente entity)
    {
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@AssociazioneId", entity.AssociazioneId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Username", entity.Username, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Email", entity.Email, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@PasswordHash", entity.PasswordHash, SqlDbType.NVarChar, 500));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Salt", entity.Salt, SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Nome", entity.Nome, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Cognome", entity.Cognome, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CodiceFiscale", entity.CodiceFiscale, SqlDbType.NVarChar, 16));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataNascita", entity.DataNascita, SqlDbType.Date));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@LuogoNascita", entity.LuogoNascita, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Telefono", entity.Telefono, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TelefonoCellulare", entity.TelefonoCellulare, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TwoFactorEnabled", entity.TwoFactorEnabled, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TwoFactorSecret", entity.TwoFactorSecret, SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TentativiAccessoFalliti", entity.TentativiAccessoFalliti, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@AccountBloccato", entity.AccountBloccato, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Attivo", entity.Attivo, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@EmailConfermata", entity.EmailConfermata, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TokenConfermaEmail", entity.TokenConfermaEmail, SqlDbType.NVarChar, 200));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataInserimento", entity.DataInserimento, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteInserimento", entity.UtenteInserimento, SqlDbType.NVarChar, 100));
    }

    private void AddParametersForUpdate(IDbCommand command, Utente entity, int tenantId)
    {
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Id", entity.UtenteId, SqlDbType.Int));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Username", entity.Username, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Email", entity.Email, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Nome", entity.Nome, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Cognome", entity.Cognome, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@CodiceFiscale", entity.CodiceFiscale, SqlDbType.NVarChar, 16));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataNascita", entity.DataNascita, SqlDbType.Date));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@LuogoNascita", entity.LuogoNascita, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Telefono", entity.Telefono, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TelefonoCellulare", entity.TelefonoCellulare, SqlDbType.NVarChar, 20));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TwoFactorEnabled", entity.TwoFactorEnabled, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@Attivo", entity.Attivo, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@EmailConfermata", entity.EmailConfermata, SqlDbType.Bit));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@DataModifica", entity.DataModifica, SqlDbType.DateTime));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@UtenteModifica", entity.UtenteModifica, SqlDbType.NVarChar, 100));
        command.Parameters.Add(SqlParameterHelper.CreateParameter("@TenantId", tenantId, SqlDbType.Int));
    }
}
