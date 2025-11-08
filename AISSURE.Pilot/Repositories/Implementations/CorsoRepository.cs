using AISSURE.Pilot.Data;
using AISSURE.Pilot.Models.Entities;
using AISSURE.Pilot.Repositories.Interfaces;
using AISSURE.Pilot.Utilities;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AISSURE.Pilot.Repositories.Implementations;

/// <summary>
/// Repository per Corsi usando ADO.NET
/// </summary>
public class CorsoRepository : BaseRepository<Corso>, ICorsoRepository
{
    protected override string TableName => "Corsi";

    public CorsoRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override async Task<IEnumerable<Corso>> GetAllAsync(int tenantId)
    {
        var corsi = new List<Corso>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Corsi
            WHERE AssociazioneId = @TenantId
            ORDER BY DataInizio DESC, Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            corsi.Add(MapFromReader(reader));
        }

        return corsi;
    }

    public override async Task<Corso?> GetByIdAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Corsi
            WHERE CorsoId = @CorsoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@CorsoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Corso>> GetCorsiAttiviAsync(int tenantId)
    {
        var corsi = new List<Corso>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Corsi
            WHERE AssociazioneId = @TenantId AND Attivo = 1
            ORDER BY DataInizio DESC, Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            corsi.Add(MapFromReader(reader));
        }

        return corsi;
    }

    public async Task<IEnumerable<Corso>> GetByAnnoSportivoAsync(string annoSportivo, int tenantId)
    {
        var corsi = new List<Corso>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Corsi
            WHERE AssociazioneId = @TenantId AND AnnoSportivo = @AnnoSportivo
            ORDER BY DataInizio, Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@AnnoSportivo", annoSportivo));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            corsi.Add(MapFromReader(reader));
        }

        return corsi;
    }

    public async Task<IEnumerable<Corso>> GetByIstruttoreAsync(int istruttoreId, int tenantId)
    {
        var corsi = new List<Corso>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Corsi
            WHERE AssociazioneId = @TenantId AND IstruttoreId = @IstruttoreId
            ORDER BY DataInizio DESC, Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@IstruttoreId", istruttoreId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            corsi.Add(MapFromReader(reader));
        }

        return corsi;
    }

    public async Task<IEnumerable<Corso>> GetByCategoriaAsync(string categoria, int tenantId)
    {
        var corsi = new List<Corso>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Corsi
            WHERE AssociazioneId = @TenantId AND Categoria = @Categoria
            ORDER BY DataInizio DESC, Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@Categoria", categoria));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            corsi.Add(MapFromReader(reader));
        }

        return corsi;
    }

    public async Task<bool> HasPostiDisponibiliAsync(int corsoId, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT PostiDisponibili, PostiOccupati
            FROM Corsi
            WHERE CorsoId = @CorsoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@CorsoId", corsoId));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var postiDisponibili = SqlParameterHelper.GetValue<int?>(reader, "PostiDisponibili");
            var postiOccupati = SqlParameterHelper.GetValue<int>(reader, "PostiOccupati");

            // Se PostiDisponibili è NULL, non ci sono limiti
            if (postiDisponibili == null)
                return true;

            return postiOccupati < postiDisponibili.Value;
        }

        return false;
    }

    public async Task IncrementPostiOccupatiAsync(int corsoId, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Corsi
            SET PostiOccupati = PostiOccupati + 1,
                DataModifica = GETUTCDATE()
            WHERE CorsoId = @CorsoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@CorsoId", corsoId));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        await ((SqlCommand)command).ExecuteNonQueryAsync();
    }

    public async Task DecrementPostiOccupatiAsync(int corsoId, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Corsi
            SET PostiOccupati = CASE
                WHEN PostiOccupati > 0 THEN PostiOccupati - 1
                ELSE 0
                END,
                DataModifica = GETUTCDATE()
            WHERE CorsoId = @CorsoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@CorsoId", corsoId));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        await ((SqlCommand)command).ExecuteNonQueryAsync();
    }

    public override async Task<int> InsertAsync(Corso entity, int tenantId)
    {
        entity.AssociazioneId = tenantId;
        SetInsertAuditFields(entity);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Corsi (
                AssociazioneId, Nome, Descrizione, Categoria, Livello,
                DataInizio, DataFine, AnnoSportivo,
                GiorniSettimana, OrarioInizio, OrarioFine,
                PostiDisponibili, PostiOccupati, ListaAttesaAttiva,
                IstruttoreId, PrezzoPieno, PrezzoRidotto, DescrizioneRiduzione,
                ImmagineUrl, Attivo, PubblicatoOnline,
                DataInserimento, UtenteInserimento
            ) VALUES (
                @AssociazioneId, @Nome, @Descrizione, @Categoria, @Livello,
                @DataInizio, @DataFine, @AnnoSportivo,
                @GiorniSettimana, @OrarioInizio, @OrarioFine,
                @PostiDisponibili, @PostiOccupati, @ListaAttesaAttiva,
                @IstruttoreId, @PrezzoPieno, @PrezzoRidotto, @DescrizioneRiduzione,
                @ImmagineUrl, @Attivo, @PubblicatoOnline,
                @DataInserimento, @UtenteInserimento
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        AddInsertParams((SqlCommand)command, entity);

        var result = await ((SqlCommand)command).ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public override async Task<bool> UpdateAsync(Corso entity, int tenantId)
    {
        entity.AssociazioneId = tenantId;
        SetUpdateAuditFields(entity);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Corsi SET
                Nome = @Nome,
                Descrizione = @Descrizione,
                Categoria = @Categoria,
                Livello = @Livello,
                DataInizio = @DataInizio,
                DataFine = @DataFine,
                AnnoSportivo = @AnnoSportivo,
                GiorniSettimana = @GiorniSettimana,
                OrarioInizio = @OrarioInizio,
                OrarioFine = @OrarioFine,
                PostiDisponibili = @PostiDisponibili,
                ListaAttesaAttiva = @ListaAttesaAttiva,
                IstruttoreId = @IstruttoreId,
                PrezzoPieno = @PrezzoPieno,
                PrezzoRidotto = @PrezzoRidotto,
                DescrizioneRiduzione = @DescrizioneRiduzione,
                ImmagineUrl = @ImmagineUrl,
                Attivo = @Attivo,
                PubblicatoOnline = @PubblicatoOnline,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE CorsoId = @CorsoId AND AssociazioneId = @AssociazioneId";

        command.Parameters.Add(new SqlParameter("@CorsoId", entity.CorsoId));
        command.Parameters.Add(new SqlParameter("@AssociazioneId", entity.AssociazioneId));
        command.Parameters.Add(new SqlParameter("@Nome", entity.Nome));
        command.Parameters.Add(new SqlParameter("@Descrizione", (object?)entity.Descrizione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Categoria", (object?)entity.Categoria ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Livello", (object?)entity.Livello ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataInizio", entity.DataInizio));
        command.Parameters.Add(new SqlParameter("@DataFine", entity.DataFine));
        command.Parameters.Add(new SqlParameter("@AnnoSportivo", (object?)entity.AnnoSportivo ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@GiorniSettimana", (object?)entity.GiorniSettimana ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@OrarioInizio", (object?)entity.OrarioInizio ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@OrarioFine", (object?)entity.OrarioFine ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PostiDisponibili", (object?)entity.PostiDisponibili ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@ListaAttesaAttiva", entity.ListaAttesaAttiva));
        command.Parameters.Add(new SqlParameter("@IstruttoreId", (object?)entity.IstruttoreId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PrezzoPieno", (object?)entity.PrezzoPieno ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PrezzoRidotto", (object?)entity.PrezzoRidotto ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DescrizioneRiduzione", (object?)entity.DescrizioneRiduzione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@ImmagineUrl", (object?)entity.ImmagineUrl ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Attivo", entity.Attivo));
        command.Parameters.Add(new SqlParameter("@PubblicatoOnline", entity.PubblicatoOnline));
        command.Parameters.Add(new SqlParameter("@DataModifica", entity.DataModifica));
        command.Parameters.Add(new SqlParameter("@UtenteModifica", (object?)entity.UtenteModifica ?? DBNull.Value));

        var rowsAffected = await ((SqlCommand)command).ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public override async Task<bool> DeleteAsync(int id, int tenantId)
    {
        // Soft delete: imposta Attivo = 0
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Corsi
            SET Attivo = 0, DataModifica = GETUTCDATE()
            WHERE CorsoId = @CorsoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@CorsoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        var rowsAffected = await ((SqlCommand)command).ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public override async Task<bool> ExistsAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT COUNT(1) FROM Corsi
            WHERE CorsoId = @CorsoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@CorsoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        var count = (int)(await ((SqlCommand)command).ExecuteScalarAsync() ?? 0);
        return count > 0;
    }

    private static Corso MapFromReader(IDataReader reader) => new()
    {
        CorsoId = SqlParameterHelper.GetValue<int>(reader, "CorsoId"),
        AssociazioneId = SqlParameterHelper.GetValue<int>(reader, "AssociazioneId"),
        Nome = SqlParameterHelper.GetStringValue(reader, "Nome"),
        Descrizione = SqlParameterHelper.GetValue<string?>(reader, "Descrizione"),
        Categoria = SqlParameterHelper.GetValue<string?>(reader, "Categoria"),
        Livello = SqlParameterHelper.GetValue<string?>(reader, "Livello"),
        DataInizio = SqlParameterHelper.GetValue<DateTime>(reader, "DataInizio"),
        DataFine = SqlParameterHelper.GetValue<DateTime>(reader, "DataFine"),
        AnnoSportivo = SqlParameterHelper.GetValue<string?>(reader, "AnnoSportivo"),
        GiorniSettimana = SqlParameterHelper.GetValue<string?>(reader, "GiorniSettimana"),
        OrarioInizio = SqlParameterHelper.GetValue<TimeSpan?>(reader, "OrarioInizio"),
        OrarioFine = SqlParameterHelper.GetValue<TimeSpan?>(reader, "OrarioFine"),
        PostiDisponibili = SqlParameterHelper.GetValue<int?>(reader, "PostiDisponibili"),
        PostiOccupati = SqlParameterHelper.GetValue<int>(reader, "PostiOccupati"),
        ListaAttesaAttiva = SqlParameterHelper.GetValue<bool>(reader, "ListaAttesaAttiva"),
        IstruttoreId = SqlParameterHelper.GetValue<int?>(reader, "IstruttoreId"),
        PrezzoPieno = SqlParameterHelper.GetValue<decimal?>(reader, "PrezzoPieno"),
        PrezzoRidotto = SqlParameterHelper.GetValue<decimal?>(reader, "PrezzoRidotto"),
        DescrizioneRiduzione = SqlParameterHelper.GetValue<string?>(reader, "DescrizioneRiduzione"),
        ImmagineUrl = SqlParameterHelper.GetValue<string?>(reader, "ImmagineUrl"),
        Attivo = SqlParameterHelper.GetValue<bool>(reader, "Attivo"),
        PubblicatoOnline = SqlParameterHelper.GetValue<bool>(reader, "PubblicatoOnline"),
        DataInserimento = SqlParameterHelper.GetValue<DateTime>(reader, "DataInserimento"),
        DataModifica = SqlParameterHelper.GetValue<DateTime?>(reader, "DataModifica"),
        UtenteInserimento = SqlParameterHelper.GetStringValue(reader, "UtenteInserimento"),
        UtenteModifica = SqlParameterHelper.GetValue<string?>(reader, "UtenteModifica")
    };

    private static void AddInsertParams(SqlCommand command, Corso entity)
    {
        command.Parameters.Add(new SqlParameter("@AssociazioneId", entity.AssociazioneId));
        command.Parameters.Add(new SqlParameter("@Nome", entity.Nome));
        command.Parameters.Add(new SqlParameter("@Descrizione", (object?)entity.Descrizione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Categoria", (object?)entity.Categoria ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Livello", (object?)entity.Livello ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataInizio", entity.DataInizio));
        command.Parameters.Add(new SqlParameter("@DataFine", entity.DataFine));
        command.Parameters.Add(new SqlParameter("@AnnoSportivo", (object?)entity.AnnoSportivo ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@GiorniSettimana", (object?)entity.GiorniSettimana ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@OrarioInizio", (object?)entity.OrarioInizio ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@OrarioFine", (object?)entity.OrarioFine ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PostiDisponibili", (object?)entity.PostiDisponibili ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PostiOccupati", entity.PostiOccupati));
        command.Parameters.Add(new SqlParameter("@ListaAttesaAttiva", entity.ListaAttesaAttiva));
        command.Parameters.Add(new SqlParameter("@IstruttoreId", (object?)entity.IstruttoreId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PrezzoPieno", (object?)entity.PrezzoPieno ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PrezzoRidotto", (object?)entity.PrezzoRidotto ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DescrizioneRiduzione", (object?)entity.DescrizioneRiduzione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@ImmagineUrl", (object?)entity.ImmagineUrl ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Attivo", entity.Attivo));
        command.Parameters.Add(new SqlParameter("@PubblicatoOnline", entity.PubblicatoOnline));
        command.Parameters.Add(new SqlParameter("@DataInserimento", entity.DataInserimento));
        command.Parameters.Add(new SqlParameter("@UtenteInserimento", entity.UtenteInserimento));
    }
}
