using NugoloASD.Web.Data;
using NugoloASD.Web.Models.Entities;
using NugoloASD.Web.Repositories.Interfaces;
using NugoloASD.Web.Utilities;
using System.Data;
using Microsoft.Data.SqlClient;

namespace NugoloASD.Web.Repositories.Implementations;

/// <summary>
/// Repository per Tesseramenti usando ADO.NET
/// </summary>
public class TesseramentoRepository : BaseRepository<Tesseramento>, ITesseramentoRepository
{
    protected override string TableName => "Tesseramenti";

    public TesseramentoRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override async Task<IEnumerable<Tesseramento>> GetAllAsync(int tenantId)
    {
        var tesseramenti = new List<Tesseramento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Tesseramenti
            WHERE AssociazioneId = @TenantId
            ORDER BY DataScadenza DESC, NumeroTessera";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tesseramenti.Add(MapFromReader(reader));
        }

        return tesseramenti;
    }

    public override async Task<Tesseramento?> GetByIdAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Tesseramenti
            WHERE TesseramentoId = @TesseramentoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@TesseramentoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Tesseramento>> GetBySocioAsync(int socioId, int tenantId)
    {
        var tesseramenti = new List<Tesseramento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Tesseramenti
            WHERE AssociazioneId = @TenantId AND SocioId = @SocioId
            ORDER BY DataScadenza DESC";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@SocioId", socioId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tesseramenti.Add(MapFromReader(reader));
        }

        return tesseramenti;
    }

    public async Task<IEnumerable<Tesseramento>> GetByFederazioneAsync(int federazioneId, int tenantId)
    {
        var tesseramenti = new List<Tesseramento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Tesseramenti
            WHERE AssociazioneId = @TenantId AND FederazioneId = @FederazioneId
            ORDER BY DataScadenza DESC, NumeroTessera";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@FederazioneId", federazioneId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tesseramenti.Add(MapFromReader(reader));
        }

        return tesseramenti;
    }

    public async Task<IEnumerable<Tesseramento>> GetByAnnoSportivoAsync(string annoSportivo, int tenantId)
    {
        var tesseramenti = new List<Tesseramento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Tesseramenti
            WHERE AssociazioneId = @TenantId AND AnnoSportivo = @AnnoSportivo
            ORDER BY NumeroTessera";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@AnnoSportivo", annoSportivo));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tesseramenti.Add(MapFromReader(reader));
        }

        return tesseramenti;
    }

    public async Task<IEnumerable<Tesseramento>> GetByStatoAsync(string stato, int tenantId)
    {
        var tesseramenti = new List<Tesseramento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Tesseramenti
            WHERE AssociazioneId = @TenantId AND StatoTesseramento = @Stato
            ORDER BY DataScadenza DESC";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@Stato", stato));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tesseramenti.Add(MapFromReader(reader));
        }

        return tesseramenti;
    }

    public async Task<IEnumerable<Tesseramento>> GetTesseramentiInScadenzaAsync(int giorniPrimaScadenza, int tenantId)
    {
        var tesseramenti = new List<Tesseramento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Tesseramenti
            WHERE AssociazioneId = @TenantId
                AND StatoTesseramento = 'Attivo'
                AND DataScadenza <= DATEADD(day, @Giorni, GETDATE())
                AND DataScadenza >= GETDATE()
            ORDER BY DataScadenza, NumeroTessera";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@Giorni", giorniPrimaScadenza));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tesseramenti.Add(MapFromReader(reader));
        }

        return tesseramenti;
    }

    public async Task<Tesseramento?> GetByNumeroTesseraAsync(string numeroTessera, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Tesseramenti
            WHERE AssociazioneId = @TenantId AND NumeroTessera = @NumeroTessera
            ORDER BY DataScadenza DESC";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@NumeroTessera", numeroTessera));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<bool> HasTesseramentoAttivoAsync(int socioId, int federazioneId, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT COUNT(1) FROM Tesseramenti
            WHERE AssociazioneId = @TenantId
                AND SocioId = @SocioId
                AND FederazioneId = @FederazioneId
                AND StatoTesseramento = 'Attivo'
                AND DataScadenza >= GETDATE()";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@SocioId", socioId));
        command.Parameters.Add(new SqlParameter("@FederazioneId", federazioneId));

        var count = (int)(await ((SqlCommand)command).ExecuteScalarAsync() ?? 0);
        return count > 0;
    }

    public override async Task<int> InsertAsync(Tesseramento entity, int tenantId)
    {
        entity.AssociazioneId = tenantId;
        SetInsertAuditFields(entity);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Tesseramenti (
                AssociazioneId, SocioId, FederazioneId,
                NumeroTessera, AnnoSportivo,
                TipoTessera, Categoria, Qualifica,
                DataEmissione, DataScadenza,
                Importo, Pagato, DataPagamento,
                StatoTesseramento, DocumentoTesseraUrl, Note,
                DataInserimento, UtenteInserimento
            ) VALUES (
                @AssociazioneId, @SocioId, @FederazioneId,
                @NumeroTessera, @AnnoSportivo,
                @TipoTessera, @Categoria, @Qualifica,
                @DataEmissione, @DataScadenza,
                @Importo, @Pagato, @DataPagamento,
                @StatoTesseramento, @DocumentoTesseraUrl, @Note,
                @DataInserimento, @UtenteInserimento
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        AddInsertParams((SqlCommand)command, entity);

        var result = await ((SqlCommand)command).ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public override async Task<bool> UpdateAsync(Tesseramento entity, int tenantId)
    {
        entity.AssociazioneId = tenantId;
        SetUpdateAuditFields(entity);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Tesseramenti SET
                SocioId = @SocioId,
                FederazioneId = @FederazioneId,
                NumeroTessera = @NumeroTessera,
                AnnoSportivo = @AnnoSportivo,
                TipoTessera = @TipoTessera,
                Categoria = @Categoria,
                Qualifica = @Qualifica,
                DataEmissione = @DataEmissione,
                DataScadenza = @DataScadenza,
                Importo = @Importo,
                Pagato = @Pagato,
                DataPagamento = @DataPagamento,
                StatoTesseramento = @StatoTesseramento,
                DocumentoTesseraUrl = @DocumentoTesseraUrl,
                Note = @Note,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE TesseramentoId = @TesseramentoId AND AssociazioneId = @AssociazioneId";

        command.Parameters.Add(new SqlParameter("@TesseramentoId", entity.TesseramentoId));
        command.Parameters.Add(new SqlParameter("@AssociazioneId", entity.AssociazioneId));
        command.Parameters.Add(new SqlParameter("@SocioId", entity.SocioId));
        command.Parameters.Add(new SqlParameter("@FederazioneId", entity.FederazioneId));
        command.Parameters.Add(new SqlParameter("@NumeroTessera", entity.NumeroTessera));
        command.Parameters.Add(new SqlParameter("@AnnoSportivo", entity.AnnoSportivo));
        command.Parameters.Add(new SqlParameter("@TipoTessera", (object?)entity.TipoTessera ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Categoria", (object?)entity.Categoria ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Qualifica", (object?)entity.Qualifica ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataEmissione", entity.DataEmissione));
        command.Parameters.Add(new SqlParameter("@DataScadenza", entity.DataScadenza));
        command.Parameters.Add(new SqlParameter("@Importo", (object?)entity.Importo ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Pagato", entity.Pagato));
        command.Parameters.Add(new SqlParameter("@DataPagamento", (object?)entity.DataPagamento ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@StatoTesseramento", entity.StatoTesseramento));
        command.Parameters.Add(new SqlParameter("@DocumentoTesseraUrl", (object?)entity.DocumentoTesseraUrl ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Note", (object?)entity.Note ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataModifica", entity.DataModifica));
        command.Parameters.Add(new SqlParameter("@UtenteModifica", (object?)entity.UtenteModifica ?? DBNull.Value));

        var rowsAffected = await ((SqlCommand)command).ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public override async Task<bool> DeleteAsync(int id, int tenantId)
    {
        // Soft delete: cambia stato in Annullato
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Tesseramenti
            SET StatoTesseramento = 'Annullato', DataModifica = GETUTCDATE()
            WHERE TesseramentoId = @TesseramentoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@TesseramentoId", id));
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
            SELECT COUNT(1) FROM Tesseramenti
            WHERE TesseramentoId = @TesseramentoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@TesseramentoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        var count = (int)(await ((SqlCommand)command).ExecuteScalarAsync() ?? 0);
        return count > 0;
    }

    private static Tesseramento MapFromReader(IDataReader reader) => new()
    {
        TesseramentoId = SqlParameterHelper.GetValue<int>(reader, "TesseramentoId"),
        AssociazioneId = SqlParameterHelper.GetValue<int>(reader, "AssociazioneId"),
        SocioId = SqlParameterHelper.GetValue<int>(reader, "SocioId"),
        FederazioneId = SqlParameterHelper.GetValue<int>(reader, "FederazioneId"),
        NumeroTessera = SqlParameterHelper.GetStringValue(reader, "NumeroTessera"),
        AnnoSportivo = SqlParameterHelper.GetStringValue(reader, "AnnoSportivo"),
        TipoTessera = SqlParameterHelper.GetValue<string?>(reader, "TipoTessera"),
        Categoria = SqlParameterHelper.GetValue<string?>(reader, "Categoria"),
        Qualifica = SqlParameterHelper.GetValue<string?>(reader, "Qualifica"),
        DataEmissione = SqlParameterHelper.GetValue<DateTime>(reader, "DataEmissione"),
        DataScadenza = SqlParameterHelper.GetValue<DateTime>(reader, "DataScadenza"),
        Importo = SqlParameterHelper.GetValue<decimal?>(reader, "Importo"),
        Pagato = SqlParameterHelper.GetValue<bool>(reader, "Pagato"),
        DataPagamento = SqlParameterHelper.GetValue<DateTime?>(reader, "DataPagamento"),
        StatoTesseramento = SqlParameterHelper.GetStringValue(reader, "StatoTesseramento"),
        DocumentoTesseraUrl = SqlParameterHelper.GetValue<string?>(reader, "DocumentoTesseraUrl"),
        Note = SqlParameterHelper.GetValue<string?>(reader, "Note"),
        DataInserimento = SqlParameterHelper.GetValue<DateTime>(reader, "DataInserimento"),
        DataModifica = SqlParameterHelper.GetValue<DateTime?>(reader, "DataModifica"),
        UtenteInserimento = SqlParameterHelper.GetStringValue(reader, "UtenteInserimento"),
        UtenteModifica = SqlParameterHelper.GetValue<string?>(reader, "UtenteModifica")
    };

    private static void AddInsertParams(SqlCommand command, Tesseramento entity)
    {
        command.Parameters.Add(new SqlParameter("@AssociazioneId", entity.AssociazioneId));
        command.Parameters.Add(new SqlParameter("@SocioId", entity.SocioId));
        command.Parameters.Add(new SqlParameter("@FederazioneId", entity.FederazioneId));
        command.Parameters.Add(new SqlParameter("@NumeroTessera", entity.NumeroTessera));
        command.Parameters.Add(new SqlParameter("@AnnoSportivo", entity.AnnoSportivo));
        command.Parameters.Add(new SqlParameter("@TipoTessera", (object?)entity.TipoTessera ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Categoria", (object?)entity.Categoria ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Qualifica", (object?)entity.Qualifica ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataEmissione", entity.DataEmissione));
        command.Parameters.Add(new SqlParameter("@DataScadenza", entity.DataScadenza));
        command.Parameters.Add(new SqlParameter("@Importo", (object?)entity.Importo ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Pagato", entity.Pagato));
        command.Parameters.Add(new SqlParameter("@DataPagamento", (object?)entity.DataPagamento ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@StatoTesseramento", entity.StatoTesseramento));
        command.Parameters.Add(new SqlParameter("@DocumentoTesseraUrl", (object?)entity.DocumentoTesseraUrl ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Note", (object?)entity.Note ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataInserimento", entity.DataInserimento));
        command.Parameters.Add(new SqlParameter("@UtenteInserimento", entity.UtenteInserimento));
    }
}
