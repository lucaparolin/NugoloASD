using AISSURE.Pilot.Data;
using AISSURE.Pilot.Models.Entities;
using AISSURE.Pilot.Repositories.Interfaces;
using AISSURE.Pilot.Utilities;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AISSURE.Pilot.Repositories.Implementations;

/// <summary>
/// Repository per Pagamenti usando ADO.NET
/// </summary>
public class PagamentoRepository : BaseRepository<Pagamento>, IPagamentoRepository
{
    protected override string TableName => "Pagamenti";

    public PagamentoRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override async Task<IEnumerable<Pagamento>> GetAllAsync(int tenantId)
    {
        var pagamenti = new List<Pagamento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Pagamenti
            WHERE AssociazioneId = @TenantId
            ORDER BY DataScadenza DESC, DataInserimento DESC";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            pagamenti.Add(MapFromReader(reader));
        }

        return pagamenti;
    }

    public override async Task<Pagamento?> GetByIdAsync(int id, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Pagamenti
            WHERE PagamentoId = @PagamentoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@PagamentoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapFromReader(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Pagamento>> GetBySocioAsync(int socioId, int tenantId)
    {
        var pagamenti = new List<Pagamento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Pagamenti
            WHERE AssociazioneId = @TenantId AND SocioId = @SocioId
            ORDER BY DataScadenza DESC, DataInserimento DESC";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@SocioId", socioId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            pagamenti.Add(MapFromReader(reader));
        }

        return pagamenti;
    }

    public async Task<IEnumerable<Pagamento>> GetByStatoAsync(string stato, int tenantId)
    {
        var pagamenti = new List<Pagamento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Pagamenti
            WHERE AssociazioneId = @TenantId AND StatoPagamento = @Stato
            ORDER BY DataScadenza DESC";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@Stato", stato));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            pagamenti.Add(MapFromReader(reader));
        }

        return pagamenti;
    }

    public async Task<IEnumerable<Pagamento>> GetPagamentiInScadenzaAsync(int giorniPrimaScadenza, int tenantId)
    {
        var pagamenti = new List<Pagamento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Pagamenti
            WHERE AssociazioneId = @TenantId
                AND StatoPagamento IN ('In Attesa', 'Parziale')
                AND DataScadenza IS NOT NULL
                AND DataScadenza <= DATEADD(day, @Giorni, GETDATE())
                AND DataScadenza >= GETDATE()
            ORDER BY DataScadenza";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@Giorni", giorniPrimaScadenza));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            pagamenti.Add(MapFromReader(reader));
        }

        return pagamenti;
    }

    public async Task<IEnumerable<Pagamento>> GetPagamentiScadutiAsync(int tenantId)
    {
        var pagamenti = new List<Pagamento>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT * FROM Pagamenti
            WHERE AssociazioneId = @TenantId
                AND StatoPagamento IN ('In Attesa', 'Parziale', 'Scaduto')
                AND DataScadenza IS NOT NULL
                AND DataScadenza < GETDATE()
            ORDER BY DataScadenza";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        using var reader = await ((SqlCommand)command).ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            pagamenti.Add(MapFromReader(reader));
        }

        return pagamenti;
    }

    public async Task<decimal> GetTotaleIncassatoAsync(DateTime dataInizio, DateTime dataFine, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT ISNULL(SUM(ImportoPagato), 0)
            FROM Pagamenti
            WHERE AssociazioneId = @TenantId
                AND DataPagamento >= @DataInizio
                AND DataPagamento <= @DataFine";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@DataInizio", dataInizio));
        command.Parameters.Add(new SqlParameter("@DataFine", dataFine));

        var result = await ((SqlCommand)command).ExecuteScalarAsync();
        return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
    }

    public async Task<decimal> GetTotaleDaIncassareAsync(DateTime dataInizio, DateTime dataFine, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT ISNULL(SUM(ImportoResiduo), 0)
            FROM Pagamenti
            WHERE AssociazioneId = @TenantId
                AND StatoPagamento IN ('In Attesa', 'Parziale', 'Scaduto')
                AND DataScadenza >= @DataInizio
                AND DataScadenza <= @DataFine";
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@DataInizio", dataInizio));
        command.Parameters.Add(new SqlParameter("@DataFine", dataFine));

        var result = await ((SqlCommand)command).ExecuteScalarAsync();
        return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
    }

    public async Task<bool> RegistraPagamentoAsync(int pagamentoId, decimal importoPagato, string metodoPagamento, string? riferimentoTransazione, int tenantId)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Pagamenti
            SET ImportoPagato = ImportoPagato + @ImportoPagato,
                MetodoPagamento = @MetodoPagamento,
                RiferimentoTransazione = @RiferimentoTransazione,
                DataPagamento = CASE
                    WHEN (ImportoPagato + @ImportoPagato) >= Importo THEN GETUTCDATE()
                    ELSE DataPagamento
                END,
                StatoPagamento = CASE
                    WHEN (ImportoPagato + @ImportoPagato) >= Importo THEN 'Pagato'
                    WHEN (ImportoPagato + @ImportoPagato) > 0 THEN 'Parziale'
                    ELSE StatoPagamento
                END,
                DataModifica = GETUTCDATE()
            WHERE PagamentoId = @PagamentoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@PagamentoId", pagamentoId));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));
        command.Parameters.Add(new SqlParameter("@ImportoPagato", importoPagato));
        command.Parameters.Add(new SqlParameter("@MetodoPagamento", metodoPagamento));
        command.Parameters.Add(new SqlParameter("@RiferimentoTransazione", (object?)riferimentoTransazione ?? DBNull.Value));

        var rowsAffected = await ((SqlCommand)command).ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public override async Task<int> InsertAsync(Pagamento entity, int tenantId)
    {
        entity.AssociazioneId = tenantId;
        SetInsertAuditFields(entity);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Pagamenti (
                AssociazioneId, SocioId,
                Causale, Descrizione, TipoPagamento,
                IscrizioneId, TesseramentoId, PrenotazioneId, AbbonamentoId,
                Importo, ImportoPagato,
                MetodoPagamento, RiferimentoTransazione,
                DataScadenza, DataPagamento,
                StatoPagamento,
                PagamentoRateale, NumeroRata, TotaleRate,
                Note,
                DataInserimento, UtenteInserimento
            ) VALUES (
                @AssociazioneId, @SocioId,
                @Causale, @Descrizione, @TipoPagamento,
                @IscrizioneId, @TesseramentoId, @PrenotazioneId, @AbbonamentoId,
                @Importo, @ImportoPagato,
                @MetodoPagamento, @RiferimentoTransazione,
                @DataScadenza, @DataPagamento,
                @StatoPagamento,
                @PagamentoRateale, @NumeroRata, @TotaleRate,
                @Note,
                @DataInserimento, @UtenteInserimento
            );
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        AddInsertParams((SqlCommand)command, entity);

        var result = await ((SqlCommand)command).ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public override async Task<bool> UpdateAsync(Pagamento entity, int tenantId)
    {
        entity.AssociazioneId = tenantId;
        SetUpdateAuditFields(entity);

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Pagamenti SET
                SocioId = @SocioId,
                Causale = @Causale,
                Descrizione = @Descrizione,
                TipoPagamento = @TipoPagamento,
                IscrizioneId = @IscrizioneId,
                TesseramentoId = @TesseramentoId,
                PrenotazioneId = @PrenotazioneId,
                AbbonamentoId = @AbbonamentoId,
                Importo = @Importo,
                ImportoPagato = @ImportoPagato,
                MetodoPagamento = @MetodoPagamento,
                RiferimentoTransazione = @RiferimentoTransazione,
                DataScadenza = @DataScadenza,
                DataPagamento = @DataPagamento,
                StatoPagamento = @StatoPagamento,
                PagamentoRateale = @PagamentoRateale,
                NumeroRata = @NumeroRata,
                TotaleRate = @TotaleRate,
                Note = @Note,
                DataModifica = @DataModifica,
                UtenteModifica = @UtenteModifica
            WHERE PagamentoId = @PagamentoId AND AssociazioneId = @AssociazioneId";

        command.Parameters.Add(new SqlParameter("@PagamentoId", entity.PagamentoId));
        command.Parameters.Add(new SqlParameter("@AssociazioneId", entity.AssociazioneId));
        command.Parameters.Add(new SqlParameter("@SocioId", entity.SocioId));
        command.Parameters.Add(new SqlParameter("@Causale", entity.Causale));
        command.Parameters.Add(new SqlParameter("@Descrizione", (object?)entity.Descrizione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TipoPagamento", (object?)entity.TipoPagamento ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@IscrizioneId", (object?)entity.IscrizioneId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TesseramentoId", (object?)entity.TesseramentoId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PrenotazioneId", (object?)entity.PrenotazioneId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@AbbonamentoId", (object?)entity.AbbonamentoId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Importo", entity.Importo));
        command.Parameters.Add(new SqlParameter("@ImportoPagato", entity.ImportoPagato));
        command.Parameters.Add(new SqlParameter("@MetodoPagamento", (object?)entity.MetodoPagamento ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@RiferimentoTransazione", (object?)entity.RiferimentoTransazione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataScadenza", (object?)entity.DataScadenza ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataPagamento", (object?)entity.DataPagamento ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@StatoPagamento", entity.StatoPagamento));
        command.Parameters.Add(new SqlParameter("@PagamentoRateale", entity.PagamentoRateale));
        command.Parameters.Add(new SqlParameter("@NumeroRata", (object?)entity.NumeroRata ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TotaleRate", (object?)entity.TotaleRate ?? DBNull.Value));
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
            UPDATE Pagamenti
            SET StatoPagamento = 'Annullato', DataModifica = GETUTCDATE()
            WHERE PagamentoId = @PagamentoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@PagamentoId", id));
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
            SELECT COUNT(1) FROM Pagamenti
            WHERE PagamentoId = @PagamentoId AND AssociazioneId = @TenantId";
        command.Parameters.Add(new SqlParameter("@PagamentoId", id));
        command.Parameters.Add(new SqlParameter("@TenantId", tenantId));

        var count = (int)(await ((SqlCommand)command).ExecuteScalarAsync() ?? 0);
        return count > 0;
    }

    private static Pagamento MapFromReader(IDataReader reader) => new()
    {
        PagamentoId = SqlParameterHelper.GetValue<int>(reader, "PagamentoId"),
        AssociazioneId = SqlParameterHelper.GetValue<int>(reader, "AssociazioneId"),
        SocioId = SqlParameterHelper.GetValue<int>(reader, "SocioId"),
        Causale = SqlParameterHelper.GetStringValue(reader, "Causale"),
        Descrizione = SqlParameterHelper.GetValue<string?>(reader, "Descrizione"),
        TipoPagamento = SqlParameterHelper.GetValue<string?>(reader, "TipoPagamento"),
        IscrizioneId = SqlParameterHelper.GetValue<int?>(reader, "IscrizioneId"),
        TesseramentoId = SqlParameterHelper.GetValue<int?>(reader, "TesseramentoId"),
        PrenotazioneId = SqlParameterHelper.GetValue<int?>(reader, "PrenotazioneId"),
        AbbonamentoId = SqlParameterHelper.GetValue<int?>(reader, "AbbonamentoId"),
        Importo = SqlParameterHelper.GetValue<decimal>(reader, "Importo"),
        ImportoPagato = SqlParameterHelper.GetValue<decimal>(reader, "ImportoPagato"),
        MetodoPagamento = SqlParameterHelper.GetValue<string?>(reader, "MetodoPagamento"),
        RiferimentoTransazione = SqlParameterHelper.GetValue<string?>(reader, "RiferimentoTransazione"),
        DataScadenza = SqlParameterHelper.GetValue<DateTime?>(reader, "DataScadenza"),
        DataPagamento = SqlParameterHelper.GetValue<DateTime?>(reader, "DataPagamento"),
        StatoPagamento = SqlParameterHelper.GetStringValue(reader, "StatoPagamento"),
        PagamentoRateale = SqlParameterHelper.GetValue<bool>(reader, "PagamentoRateale"),
        NumeroRata = SqlParameterHelper.GetValue<int?>(reader, "NumeroRata"),
        TotaleRate = SqlParameterHelper.GetValue<int?>(reader, "TotaleRate"),
        Note = SqlParameterHelper.GetValue<string?>(reader, "Note"),
        DataInserimento = SqlParameterHelper.GetValue<DateTime>(reader, "DataInserimento"),
        DataModifica = SqlParameterHelper.GetValue<DateTime?>(reader, "DataModifica"),
        UtenteInserimento = SqlParameterHelper.GetStringValue(reader, "UtenteInserimento"),
        UtenteModifica = SqlParameterHelper.GetValue<string?>(reader, "UtenteModifica")
    };

    private static void AddInsertParams(SqlCommand command, Pagamento entity)
    {
        command.Parameters.Add(new SqlParameter("@AssociazioneId", entity.AssociazioneId));
        command.Parameters.Add(new SqlParameter("@SocioId", entity.SocioId));
        command.Parameters.Add(new SqlParameter("@Causale", entity.Causale));
        command.Parameters.Add(new SqlParameter("@Descrizione", (object?)entity.Descrizione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TipoPagamento", (object?)entity.TipoPagamento ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@IscrizioneId", (object?)entity.IscrizioneId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TesseramentoId", (object?)entity.TesseramentoId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@PrenotazioneId", (object?)entity.PrenotazioneId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@AbbonamentoId", (object?)entity.AbbonamentoId ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Importo", entity.Importo));
        command.Parameters.Add(new SqlParameter("@ImportoPagato", entity.ImportoPagato));
        command.Parameters.Add(new SqlParameter("@MetodoPagamento", (object?)entity.MetodoPagamento ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@RiferimentoTransazione", (object?)entity.RiferimentoTransazione ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataScadenza", (object?)entity.DataScadenza ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataPagamento", (object?)entity.DataPagamento ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@StatoPagamento", entity.StatoPagamento));
        command.Parameters.Add(new SqlParameter("@PagamentoRateale", entity.PagamentoRateale));
        command.Parameters.Add(new SqlParameter("@NumeroRata", (object?)entity.NumeroRata ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@TotaleRate", (object?)entity.TotaleRate ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@Note", (object?)entity.Note ?? DBNull.Value));
        command.Parameters.Add(new SqlParameter("@DataInserimento", entity.DataInserimento));
        command.Parameters.Add(new SqlParameter("@UtenteInserimento", entity.UtenteInserimento));
    }
}
