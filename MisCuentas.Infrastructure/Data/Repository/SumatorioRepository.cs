using Csv;
using System.Data;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Interface.Service;
using MySql.Data.MySqlClient;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Infrastructure.Tmp.Utils;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Service;


public class SumatorioRepository : ISumatorioRepository
{
    private readonly ConexionBd conexion = new ConexionBd();
    private readonly IGestorDeErroresService _gestorDeErroresService;

    /// <summary>
    /// Repository class handling sumatorio operations such as retrieval and export functionality.
    /// </summary>
    public SumatorioRepository(IGestorDeErroresService gestorDeErroresService)
    {
        _gestorDeErroresService = gestorDeErroresService;
    }

    /// <summary>
    /// Retrieves a list of sumatorios filtered by the specified month, year, and concept identifier.
    /// </summary>
    /// <param name="mes">The month used to filter the results. Can be null for no filtering by month.</param>
    /// <param name="ano">The year used to filter the results. Can be null for no filtering by year.</param>
    /// <param name="concepto">The concept identifier used to filter the results. Can be null for no filtering by concept identifier.</param>
    /// <returns>A list of <see cref="Sumatorio"/> objects containing the filtered sumatorios.</returns>
    public List<Sumatorio> ObtenerSumatorio(int? mes, int? ano, int? concepto)
    {
        List<Sumatorio> sumatorios = new List<Sumatorio>();

        try
        {
            var conn = conexion.CrearConexion();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = Consulta.Sumatorios.sumatorio;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
            cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;
            cmd.Parameters.AddWithValue("@_concepto", MySqlDbType.Int32).Value =
                concepto.HasValue ? concepto.Value : DBNull.Value;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sumatorios.Add(new Sumatorio()
                {
                    concepto = reader.IsDBNull(0) ? "N/D" : reader.GetString(0),
                    total = reader.IsDBNull(1) ? 0 : Convert.ToDecimal(reader.GetValue(1))
                });
            }
        }
        catch (MySqlException mySqlException)
        {
            _gestorDeErroresService.GestionarExcepcionesMySQL(mySqlException);
            return null;
        }
        
        return sumatorios;
    }
}
