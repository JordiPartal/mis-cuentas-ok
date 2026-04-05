using System.Data;
using MySql.Data.MySqlClient;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Infrastructure.Tmp.Utils;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Service;

public class BalanceRepository : IBalanceRepository
{
    private readonly ConexionBd conexion = new ConexionBd();
    private readonly IGestorDeErroresService _gestorDeErroresService; // 1. La variable

    /// <summary>
    /// Provides methods for accessing and retrieving balance data from the database.
    /// Implements the <see cref="IBalanceRepository"/> interface.
    /// </summary>
    public BalanceRepository(IGestorDeErroresService gestorDeErroresService) // 2. El parámetro
    {
        _gestorDeErroresService = gestorDeErroresService; 
    }

    /// <summary>
    /// Represents the financial balance containing income, expenses, savings, and profit data.
    /// </summary>
    /// <param name="mes">The month used to filter the results. Can be null for no filtering by month.</param>
    /// <param name="ano">The year used to filter the results. Can be null for no filtering by year.</param>
    public Balance Balance(int? mes, int? ano)
    {
        var balance = new Balance();

        try
        {
            using var conn = conexion.CrearConexion();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = Consulta.Balance.balance;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
            cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;
            
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                balance.ingresos = reader.IsDBNull(0) ? 0 : Convert.ToDecimal(reader.GetValue(0));
                balance.gastos = reader.IsDBNull(1) ? 0 : Convert.ToDecimal(reader.GetValue(1));
                balance.ahorro = reader.IsDBNull(2) ? 0 : Convert.ToDecimal(reader.GetValue(2));
                balance.ganancia = reader.IsDBNull(3) ? 0 : Convert.ToDecimal(reader.GetValue(3));
            }
        }
        catch (MySqlException mySqlException)
        {
            _gestorDeErroresService.GestionarExcepcionesMySQL(mySqlException);
            return new Balance { ingresos = 0, gastos = 0, ahorro = 0, ganancia = 0 };
        }

        return balance;
    }
}