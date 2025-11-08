using AISSURE.Pilot.Data;
using AISSURE.Pilot.Models.Entities;
using AISSURE.Pilot.Repositories.Interfaces;

namespace AISSURE.Pilot.Repositories.Implementations;

/// <summary>
/// Repository per Soci usando ADO.NET
/// </summary>
public class SocioRepository : BaseRepository<Socio>, ISocioRepository
{
    protected override string TableName => "Soci";

    public SocioRepository(IDbConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public override Task<IEnumerable<Socio>> GetAllAsync(int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<Socio?> GetByIdAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<Socio?> GetByCodiceFiscaleAsync(string codiceFiscale, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<Socio?> GetByNumeroTesseraAsync(string numeroTessera, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Socio>> GetByTipoAsync(string tipoSocio, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Socio>> GetByStatoAsync(string statoSocio, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Socio>> GetSociConCertificatoInScadenzaAsync(int giorniPrimaScadenza, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public Task<IEnumerable<Socio>> SearchAsync(string searchTerm, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<int> InsertAsync(Socio entity, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<bool> UpdateAsync(Socio entity, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<bool> DeleteAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }

    public override Task<bool> ExistsAsync(int id, int tenantId)
    {
        throw new NotImplementedException("Da implementare con ADO.NET");
    }
}
