using System.Data;

namespace AISSURE.Pilot.Data;

/// <summary>
/// Factory per la creazione di connessioni al database
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Crea una nuova connessione al database
    /// </summary>
    IDbConnection CreateConnection();
}
