using System.Data;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Models;
using MisCuentas.Domain.Enums;
using MisCuentas.Infrastructure.Data;
using MySql.Data.MySqlClient;

namespace MisCuenta.Infrastructure.Repository;

public class TransaccionRepository : ITransaccionRepository
{
    private readonly ConexionBd _conexion;

    public TransaccionRepository(ConexionBd conexion) => _conexion = conexion;

    /// <summary>
    /// Retrieves a list of transactions based on the specified month and year.
    /// </summary>
    /// <param name="mes">The month for which transactions are to be retrieved. Can be null to retrieve transactions without filtering by month.</param>
    /// <param name="ano">The year for which transactions are to be retrieved. Can be null to retrieve transactions without filtering by year.</param>
    /// <returns>A list of <see cref="Transaccion"/> objects representing the retrieved transactions.</returns>
    public List<Transaccion> ObtenerTransaccion(int? mes, int? ano)
    {
        var transacciones = new List<Transaccion>();
        using var conn = _conexion.CrearConexion();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = SpTransacciones.SP_Transacciones.ToString();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
        cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;

        using var lector = cmd.ExecuteReader();
        while (lector.Read())
        {
            transacciones.Add(new Transaccion()
            {
                FechaCargo = lector.IsDBNull(0) ? DateTime.Now : lector.GetDateTime(0),
                Tipo = lector.IsDBNull(1) ? "N/D" : lector.GetString(1),
                Concepto = lector.IsDBNull(2) ? "N/D" : lector.GetString(2),
                BaseImponible = lector.IsDBNull(3) ? 0 : Convert.ToDecimal(lector.GetValue(3)),
                Cuota = lector.IsDBNull(4) ? 0 : Convert.ToDecimal(lector.GetValue(4)),
                Cantidad = lector.IsDBNull(5) ? 0 : Convert.ToDecimal(lector.GetValue(5))
            });
        }
        
        return transacciones;
    }

    /// <summary>
    /// Asynchronously retrieves a list of transactions based on the specified month and year.
    /// </summary>
    /// <param name="mes">The month for which transactions are to be retrieved. Can be null to retrieve transactions without filtering by month.</param>
    /// <param name="ano">The year for which transactions are to be retrieved. Can be null to retrieve transactions without filtering by year.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Transaccion"/> objects representing the retrieved transactions.</returns>
    public async Task<List<Transaccion>> ObtenerTransaccionAsync(int? mes, int? ano)
    {
        var id = 1;
        var transacciones = new List<Transaccion>();
        using var conn = _conexion.CrearConexion();
        await conn.OpenAsync();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = SpTransacciones.SP_Transacciones.ToString();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
        cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;

        await using var lector = await cmd.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            transacciones.Add(new Transaccion()
            {
                Id = id,
                FechaCargo = lector.IsDBNull(0) ? DateTime.Now : lector.GetDateTime(0),
                Tipo = lector.IsDBNull(1) ? "N/D" : lector.GetString(1),
                Concepto = lector.IsDBNull(2) ? "N/D" : lector.GetString(2),
                BaseImponible = lector.IsDBNull(3) ? 0 : Convert.ToDecimal(lector.GetValue(3)),
                Cuota = lector.IsDBNull(4) ? 0 : Convert.ToDecimal(lector.GetValue(4)),
                Cantidad = lector.IsDBNull(5) ? 0 : Convert.ToDecimal(lector.GetValue(5))
            });
            id++;
        }
        
        return transacciones;
    }

    /// <summary>
    /// Adds a new transaction to the data source.
    /// </summary>
    /// <param name="transaccion">The transaction object containing information such as date, type, concept, base amount, and tax amount.</param>
    public void AgregarTransaccion(Transaccion transaccion)
    {
        using var conn = _conexion.CrearConexion();
        conn.Open();
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = SpTransacciones.SP_Insertar_Transaccion.ToString();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@fecha", transaccion.FechaCargo);
        cmd.Parameters.AddWithValue("@concepto", transaccion.Concepto);
        cmd.Parameters.AddWithValue("@cantidad", transaccion.Cantidad);
        cmd.Parameters.AddWithValue("@categoria", transaccion.IdTipo);
        cmd.Parameters.AddWithValue("@impuesto", transaccion.IdImpuesto);
        
        cmd.ExecuteNonQuery();
    }
}