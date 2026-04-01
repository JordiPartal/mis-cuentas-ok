using System.Data;
using MySql.Data.MySqlClient;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Infrastructure.Tmp.Utils;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Service;

public class BalanceService : IBalanceService
{
    private readonly ConexionBd conexion = new ConexionBd();

    /// <summary>
    /// Calculates and retrieves a balance for a specific month and year.
    /// </summary>
    /// <param name="mes">The month for which the balance is requested. Can be null.</param>
    /// <param name="ano">The year for which the balance is requested. Can be null.</param>
    /// <returns>Returns a <see cref="Balance"/> object containing the balance information, or null if no data is found.</returns>
    public Balance Balance(int? mes, int? ano)
    {
        using var conn = conexion.CrearConexion();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = Consulta.Balance.balance;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
        cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;

        try
        {
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Balance()
                {
                    ingresos = reader.IsDBNull(0) ? 0 : Convert.ToDecimal(reader.GetValue(0)),
                    gastos = reader.IsDBNull(1) ? 0 : Convert.ToDecimal(reader.GetValue(1)),
                    ahorro = reader.IsDBNull(2) ? 0 : Convert.ToDecimal(reader.GetValue(2)),
                    ganancia = reader.IsDBNull(3) ? 0 : Convert.ToDecimal(reader.GetValue(3))
                };
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}