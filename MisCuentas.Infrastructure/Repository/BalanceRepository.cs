using System.Data;
using MisCuentas.Domain.Enums;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Interface.Service;
using MySql.Data.MySqlClient;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Domain.Models;

namespace MisCuenta.Infrastructure.Repository;

public class BalanceRepository : IBalanceRepository
{
    private readonly ConexionBd _conexion = new ConexionBd();
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
    /// Retrieves the financial balance information for a specific month and year asynchronously.
    /// </summary>
    /// <param name="mes">The month used to filter the balance data. If null, no filtering by month is applied.</param>
    /// <param name="ano">The year used to filter the balance data. If null, no filtering by year is applied.</param>
    /// <returns>A <see cref="Balance"/> object containing financial balance details such as income, expenses, savings, and profit.</returns>
    public async Task<Balance> BalanceAsync(int? mes, int? ano)
    {
        var balance = new Balance();

        try
        {
            await using var conn = _conexion.CrearConexion();
            await conn.OpenAsync();

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = SpBalance.SP_Balance.ToString();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
            cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;
            
            await using var lector = await cmd.ExecuteReaderAsync();
            if (await lector.ReadAsync())
            {
                balance.Ingresos = lector.IsDBNull(0) ? 0 : Convert.ToDecimal(lector.GetValue(0));
                balance.Gastos = lector.IsDBNull(1) ? 0 : Convert.ToDecimal(lector.GetValue(1));
                balance.Ahorro = lector.IsDBNull(2) ? 0 : Convert.ToDecimal(lector.GetValue(2));
                balance.Ganancia = lector.IsDBNull(3) ? 0 : Convert.ToDecimal(lector.GetValue(3));
            }
        }
        catch (MySqlException mySqlException)
        {
            _gestorDeErroresService.GestionarExcepcionesMySQL(mySqlException);
            return new Balance { Ingresos = 0, Gastos = 0, Ahorro = 0, Ganancia = 0 };
        }

        return balance;
    }

    /// <summary>
    /// Represents the financial balance, including details about income, expenses, savings, and profit.
    /// </summary>
    public Balance Balance(int? mes, int? ano)
    {
        var balance = new Balance();

        try
        {
            using var conn = _conexion.CrearConexion();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = SpBalance.SP_Balance.ToString();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
            cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;
            
            using var lector = cmd.ExecuteReader();
            if (lector.Read())
            {
                balance.Ingresos = lector.IsDBNull(0) ? 0 : Convert.ToDecimal(lector.GetValue(0));
                balance.Gastos = lector.IsDBNull(1) ? 0 : Convert.ToDecimal(lector.GetValue(1));
                balance.Ahorro = lector.IsDBNull(2) ? 0 : Convert.ToDecimal(lector.GetValue(2));
                balance.Ganancia = lector.IsDBNull(3) ? 0 : Convert.ToDecimal(lector.GetValue(3));
            }
        }
        catch (MySqlException mySqlException)
        {
            _gestorDeErroresService.GestionarExcepcionesMySQL(mySqlException);
            return new Balance { Ingresos = 0, Gastos = 0, Ahorro = 0, Ganancia = 0 };
        }

        return balance;    
    }
}