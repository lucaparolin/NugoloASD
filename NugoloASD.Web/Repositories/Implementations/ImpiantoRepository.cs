using NugoloASD.Web.Data;
using NugoloASD.Web.Models.Entities;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Utilities;
using System.Data;
using Microsoft.Data.SqlClient;

namespace NugoloASD.Web.Repositories.Implementations;

/// <summary>
/// Repository per Impianti usando ADO.NET
/// </summary>
public class ImpiantoRepository : BaseRepository<Impianto>, IImpiantoRepository
{
    protected override string TableName => "Impianti";

    public ImpiantoRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override async Task<IEnumerable<Impianto>> GetAllAsync(int tenantId)
    {
        var impianti = new List<Impianto>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Impianti
            WHERE AssociazioneId = @TenantId
            ORDER BY Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            impianti.Add(MapFromReader(reader));
        }

        return impianti;
    }

    public override async Task<Impianto?> GetByIdAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Impianti
            WHERE ImpiantoId = @ImpiantoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@ImpiantoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Impianto>> GetImpiantiAttiviAsync(int tenantId)
    {
        var impianti = new List<Impianto>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Impianti
            WHERE AssociazioneId = @TenantId AND Attivo = 1
            ORDER BY Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            impianti.Add(MapFromReader(reader));
        }

        return impianti;
    }

    public async Task<IEnumerable<Impianto>> GetByTipoImpiantoAsync(string tipoImpianto, int tenantId)
    {
        var impianti = new List<Impianto>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Impianti
            WHERE AssociazioneId = @TenantId AND TipoImpianto = @TipoImpianto
            ORDER BY Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@TipoImpianto", tipoImpianto));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            impianti.Add(MapFromReader(reader));
        }

        return impianti;
    }

    public async Task<IEnumerable<Impianto>> GetImpiantiPrenotabiliAsync(int tenantId)
    {
        var impianti = new List<Impianto>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Impianti
            WHERE AssociazioneId = @TenantId AND Attivo = 1 AND Prenotabile = 1
            ORDER BY Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            impianti.Add(MapFromReader(reader));
        }

        return impianti;
    }

    public async Task<IEnumerable<Impianto>> GetImpiantiPrenotabiliOnlineAsync(int tenantId)
    {
        var impianti = new List<Impianto>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Impianti
            WHERE AssociazioneId = @TenantId
                AND Attivo = 1
                AND Prenotabile = 1
                AND PrenotabileOnline = 1
            ORDER BY Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            impianti.Add(MapFromReader(reader));
        }

        return impianti;
    }

    public async Task<IEnumerable<Impianto>> SearchAsync(string searchTerm, int tenantId)
    {
        var impianti = new List<Impianto>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Impianti
            WHERE AssociazioneId = @TenantId
                AND (Nome LIKE @SearchTerm
                    OR TipoImpianto LIKE @SearchTerm
                    OR Citta LIKE @SearchTerm
                    OR Descrizione LIKE @SearchTerm)
            ORDER BY Nome";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@SearchTerm", $"%{searchTerm}%"));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            impianti.Add(MapFromReader(reader));
        }

        return impianti;
    }

    public override async Task<int> InsertAsync(Impianto entity, int tenantId)
    {
        entity.AssociazioneId = tenantId;
        SetInsertAuditFields(entity);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Impianti (
                AssociazioneId, Nome, Descrizione, TipoImpianto,
                Indirizzo, Citta, CAP,
                Capienza, Superficie, UnitaMisura, CopertoScoperto,
                Prenotabile, PrenotabileOnline, TempoMinimoPrenotazione,
                AnticipoPrevistaGiorni, MassimoCancellazioneOre,
                TariffaOraria, TariffaGiornaliera,
                ImmagineUrl, Attivo,
                DataInserimento, UtenteInserimento
            ) VALUES (
                @AssociazioneId, @Nome, @Descrizione, @TipoImpianto,
                @Indirizzo, @Citta, @CAP,
                @Capienza, @Superficie, @UnitaMisura, @CopertoScoperto,
                @Prenotabile, @PrenotabileOnline, @TempoMinimoPrenotazione,
                @AnticipoPrevistaGiorni, @MassimoCancellazioneOre,
                @TariffaOraria, @TariffaGiornaliera,
                @ImmagineUrl, @Attivo,
                @DataInserimento, @UtenteInserimento
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        AddInsertParams((SqlCommand)command, entity);

        var result = await ((SqlCommand)command).ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public override async Task<bool> UpdateAsync(Impianto entity, int tenantId)
    {
        entity.AssociazioneId = tenantId;
        SetUpdateAuditFields(entity);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Impianti SET
                Nome = @Nome,
                Descrizione = @Descrizione,
                TipoImpianto = @TipoImpianto,
                Indirizzo = @Indirizzo,
                Citta = @Citta,
                CAP = @CAP,
                Capienza = @Capienza,
                Superficie = @Superficie,
                UnitaMisura = @UnitaMisura,
                CopertoScoperto = @CopertoScoperto,
                Prenotabile = @Prenotabile,
                PrenotabileOnline = @PrenotabileOnline,
                TempoMinimoPrenotazione = @TempoMinimoPrenotazione,
                AnticipoPrevistaGiorni = @AnticipoPrevistaGiorni,
                MassimoCancellazioneOre = @MassimoCancellazioneOre,
                TariffaOraria = @TariffaOraria,
                TariffaGiornaliera = @TariffaGiornaliera,
                ImmagineUrl = @ImmagineUrl,
                Attivo = @Attivo,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE ImpiantoId = @ImpiantoId AND AssociazioneId = @AssociazioneId";

        command.Parameters.Add(new SqlParameter("@ImpiantoId", entity.ImpiantoId));
        command.Parameters.Add(new SqlParameter("@AssociazioneId", entity.AssociazioneId));
        command.Parameters.Add(new SqlParameter("@Nome", entity.Nome));
        command.Parameters.Add(new SqlParameter("@Descrizione", (object?)entity.Descrizione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TipoImpianto", (object?)entity.TipoImpianto ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Indirizzo", (object?)entity.Indirizzo ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Citta", (object?)entity.Citta ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@CAP", (object?)entity.CAP ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Capienza", (object?)entity.Capienza ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Superficie", (object?)entity.Superficie ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@UnitaMisura", (object?)entity.UnitaMisura ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@CopertoScoperto", (object?)entity.CopertoScoperto ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Prenotabile", entity.Prenotabile));
        command.Parameters.Add(new SqlParameter("@PrenotabileOnline", entity.PrenotabileOnline));
        command.Parameters.Add(new SqlParameter("@TempoMinimoPrenotazione", entity.TempoMinimoPrenotazione));
        command.Parameters.Add(new SqlParameter("@AnticipoPrevistaGiorni", entity.AnticipoPrevistaGiorni));
        command.Parameters.Add(new SqlParameter("@MassimoCancellazioneOre", entity.MassimoCancellazioneOre));
        command.Parameters.Add(new SqlParameter("@TariffaOraria", (object?)entity.TariffaOraria ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TariffaGiornaliera", (object?)entity.TariffaGiornaliera ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@ImmagineUrl", (object?)entity.ImmagineUrl ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Attivo", entity.Attivo));
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
            UPDATE Impianti
            SET Attivo = 0, DataModifica = GETUTCDATE()
            WHERE ImpiantoId = @ImpiantoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@ImpiantoId", id));
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
            SELECT COUNT(1) FROM Impianti
            WHERE ImpiantoId = @ImpiantoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@ImpiantoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        var count = (int)(await ((SqlCommand)command).ExecuteScalarAsync() ?? 0);
        return count > 0;
    }

    private static Impianto MapFromReader(IDataReader reader) => new()
    {
        ImpiantoId = SqlParameterHelper.GetValue<int>(reader, "ImpiantoId"),
        AssociazioneId = SqlParameterHelper.GetValue<int>(reader, "AssociazioneId"),
        Nome = SqlParameterHelper.GetStringValue(reader, "Nome"),
        Descrizione = SqlParameterHelper.GetValue<string?>(reader, "Descrizione"),
        TipoImpianto = SqlParameterHelper.GetValue<string?>(reader, "TipoImpianto"),
        Indirizzo = SqlParameterHelper.GetValue<string?>(reader, "Indirizzo"),
        Citta = SqlParameterHelper.GetValue<string?>(reader, "Citta"),
        CAP = SqlParameterHelper.GetValue<string?>(reader, "CAP"),
        Capienza = SqlParameterHelper.GetValue<int?>(reader, "Capienza"),
        Superficie = SqlParameterHelper.GetValue<decimal?>(reader, "Superficie"),
        UnitaMisura = SqlParameterHelper.GetValue<string?>(reader, "UnitaMisura"),
        CopertoScoperto = SqlParameterHelper.GetValue<string?>(reader, "CopertoScoperto"),
        Prenotabile = SqlParameterHelper.GetValue<bool>(reader, "Prenotabile"),
        PrenotabileOnline = SqlParameterHelper.GetValue<bool>(reader, "PrenotabileOnline"),
        TempoMinimoPrenotazione = SqlParameterHelper.GetValue<int>(reader, "TempoMinimoPrenotazione"),
        AnticipoPrevistaGiorni = SqlParameterHelper.GetValue<int>(reader, "AnticipoPrevistaGiorni"),
        MassimoCancellazioneOre = SqlParameterHelper.GetValue<int>(reader, "MassimoCancellazioneOre"),
        TariffaOraria = SqlParameterHelper.GetValue<decimal?>(reader, "TariffaOraria"),
        TariffaGiornaliera = SqlParameterHelper.GetValue<decimal?>(reader, "TariffaGiornaliera"),
        ImmagineUrl = SqlParameterHelper.GetValue<string?>(reader, "ImmagineUrl"),
        Attivo = SqlParameterHelper.GetValue<bool>(reader, "Attivo"),
        DataInserimento = SqlParameterHelper.GetValue<DateTime>(reader, "DataInserimento"),
        DataModifica = SqlParameterHelper.GetValue<DateTime?>(reader, "DataModifica"),
        UtenteInserimento = SqlParameterHelper.GetStringValue(reader, "UtenteInserimento"),
        UtenteModifica = SqlParameterHelper.GetValue<string?>(reader, "UtenteModifica")
    };

    private static void AddInsertParams(SqlCommand command, Impianto entity)
    {
        command.Parameters.Add(new SqlParameter("@AssociazioneId", entity.AssociazioneId));
        command.Parameters.Add(new SqlParameter("@Nome", entity.Nome));
        command.Parameters.Add(new SqlParameter("@Descrizione", (object?)entity.Descrizione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TipoImpianto", (object?)entity.TipoImpianto ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Indirizzo", (object?)entity.Indirizzo ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Citta", (object?)entity.Citta ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@CAP", (object?)entity.CAP ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Capienza", (object?)entity.Capienza ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Superficie", (object?)entity.Superficie ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@UnitaMisura", (object?)entity.UnitaMisura ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@CopertoScoperto", (object?)entity.CopertoScoperto ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Prenotabile", entity.Prenotabile));
        command.Parameters.Add(new SqlParameter("@PrenotabileOnline", entity.PrenotabileOnline));
        command.Parameters.Add(new SqlParameter("@TempoMinimoPrenotazione", entity.TempoMinimoPrenotazione));
        command.Parameters.Add(new SqlParameter("@AnticipoPrevistaGiorni", entity.AnticipoPrevistaGiorni));
        command.Parameters.Add(new SqlParameter("@MassimoCancellazioneOre", entity.MassimoCancellazioneOre));
        command.Parameters.Add(new SqlParameter("@TariffaOraria", (object?)entity.TariffaOraria ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TariffaGiornaliera", (object?)entity.TariffaGiornaliera ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@ImmagineUrl", (object?)entity.ImmagineUrl ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Attivo", entity.Attivo));
        command.Parameters.Add(new SqlParameter("@DataInserimento", entity.DataInserimento));
        command.Parameters.Add(new SqlParameter("@UtenteInserimento", entity.UtenteInserimento));
    }
}
