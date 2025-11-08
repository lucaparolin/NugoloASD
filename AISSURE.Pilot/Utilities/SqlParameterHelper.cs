using System.Data;
using System.Data.SqlClient;

namespace AISSURE.Pilot.Utilities;

/// <summary>
/// Helper per la gestione dei parametri SQL
/// </summary>
public static class SqlParameterHelper
{
    /// <summary>
    /// Crea un parametro SQL
    /// </summary>
    public static SqlParameter CreateParameter(string name, object? value, SqlDbType type)
    {
        return new SqlParameter
        {
            ParameterName = name,
            Value = value ?? DBNull.Value,
            SqlDbType = type
        };
    }

    /// <summary>
    /// Crea un parametro SQL con dimensione
    /// </summary>
    public static SqlParameter CreateParameter(string name, object? value, SqlDbType type, int size)
    {
        return new SqlParameter
        {
            ParameterName = name,
            Value = value ?? DBNull.Value,
            SqlDbType = type,
            Size = size
        };
    }

    /// <summary>
    /// Converte DBNull a null per tipi nullable
    /// </summary>
    public static T? GetValue<T>(object value)
    {
        if (value == DBNull.Value)
            return default;

        return (T)value;
    }

    /// <summary>
    /// Converte DBNull a null per stringhe
    /// </summary>
    public static string? GetStringValue(object value)
    {
        if (value == DBNull.Value)
            return null;

        return value.ToString();
    }
}
