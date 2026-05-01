using System.Data;
using MisCuentas.Domain.Enums;
using MisCuentas.Domain.Interface.Repository;
using MySql.Data.MySqlClient;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Data;

namespace MisCuenta.Infrastructure.Repository;

public class MovimientoRepository : IMovimientoRepository
{
    private readonly ConexionBd _conexion;
    
    public MovimientoRepository(ConexionBd conexion) => _conexion = conexion;

    /// <summary>
    /// Retrieves a list of movements for the specified month and year.
    /// </summary>
    /// <param name="mes">The month for which movements should be retrieved, or null to retrieve all months.</param>
    /// <param name="ano">The year for which movements should be retrieved, or null to retrieve all years.</param>
    /// <returns>A list of Movimiento objects corresponding to the specified month and year.</returns>
    public List<Movimiento> ObtenerMovimientos(int? mes, int? ano)
    {
        List<Movimiento> movimientos = new List<Movimiento>();
        using var conn = _conexion.CrearConexion();
        conn.Open();
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = SPMovimientos.SP_Movimientos.ToString();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@_mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
        cmd.Parameters.AddWithValue("@_ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            movimientos.Add(new Movimiento()
                {
                    Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    Ano = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    Mes = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    Dia = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    Cargo = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                    Tipo = reader.IsDBNull(5) ? "N/D" : reader.GetString(5),
                    Concepto = reader.IsDBNull(6) ? "N/D" : reader.GetString(6),
                    Debe = reader.IsDBNull(7) ? 0 : Convert.ToDecimal(reader.GetValue(7)),
                    Haber = reader.IsDBNull(8) ? 0 : Convert.ToDecimal(reader.GetValue(8)),
                    Saldo = reader.IsDBNull(9) ? 0 : Convert.ToDecimal(reader.GetValue(9))
                }
            );    
        }
        
        return movimientos;
    }
}