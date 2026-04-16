using System.Data;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Tmp.Utils;
using MySql.Data.MySqlClient;

namespace MisCuentas.Infrastructure.Data.Repository;

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
        cmd.CommandText = Consulta.Transacciones.obtener;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
        cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;

        using var lector = cmd.ExecuteReader();
        while (lector.Read())
        {
            transacciones.Add(new Transaccion()
            {
                fechaCargo = lector.IsDBNull(0) ? DateTime.Now : lector.GetDateTime(0),
                tipo = lector.IsDBNull(1) ? "N/D" : lector.GetString(1),
                concepto = lector.IsDBNull(2) ? "N/D" : lector.GetString(2),
                _base = lector.IsDBNull(3) ? 0 : Convert.ToDecimal(lector.GetValue(3)),
                cuota = lector.IsDBNull(4) ? 0 : Convert.ToDecimal(lector.GetValue(4)),
                cantidad = lector.IsDBNull(5) ? 0 : Convert.ToDecimal(lector.GetValue(5))
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
        cmd.CommandText = Consulta.Transacciones.obtener;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
        cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;

        await using var lector = await cmd.ExecuteReaderAsync();
        while (await lector.ReadAsync())
        {
            transacciones.Add(new Transaccion()
            {
                id = id,
                fechaCargo = lector.IsDBNull(0) ? DateTime.Now : lector.GetDateTime(0),
                tipo = lector.IsDBNull(1) ? "N/D" : lector.GetString(1),
                concepto = lector.IsDBNull(2) ? "N/D" : lector.GetString(2),
                _base = lector.IsDBNull(3) ? 0 : Convert.ToDecimal(lector.GetValue(3)),
                cuota = lector.IsDBNull(4) ? 0 : Convert.ToDecimal(lector.GetValue(4)),
                cantidad = lector.IsDBNull(5) ? 0 : Convert.ToDecimal(lector.GetValue(5))
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
        cmd.CommandText = Consulta.Transacciones.inserta;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@fecha", MySqlDbType.DateTime).Value = transaccion.fechaCargo;
        cmd.Parameters.AddWithValue("@concepto", MySqlDbType.VarChar).Value = transaccion.tipo;
        cmd.Parameters.AddWithValue("@cantidad", MySqlDbType.Decimal).Value = transaccion.concepto;
        cmd.Parameters.AddWithValue("@categoria", MySqlDbType.Int16).Value = transaccion.idtipo;
        cmd.Parameters.AddWithValue("@impuesto", MySqlDbType.Int16).Value = transaccion.idImpuesto;
        
        cmd.ExecuteNonQuery();
    }
}