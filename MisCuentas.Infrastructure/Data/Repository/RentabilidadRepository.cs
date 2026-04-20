using System.Data;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuenta.Infrastructure.Data.Repository;

public class RentabilidadRepository : IRentabilidadRepository
{
    private readonly ConexionBd _conexion;
    
    public RentabilidadRepository(ConexionBd conexion) => _conexion = conexion;

    /// <summary>
    /// Retrieves a list of profitability data from the database.
    /// </summary>
    /// <returns>
    /// A list of <c>Rentabilidad</c> objects containing profitability data such as year, month, revenue, expenses, margin, and other related financial metrics.
    /// </returns>
    public List<Rentabilidad> ObtenerRentabilidad()
    {
        var rentabilidad = new List<Rentabilidad>();
        using var conn = _conexion.CrearConexion();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = Consulta.Rentabilidad.rentabilidad;
        cmd.CommandType = CommandType.StoredProcedure;

        using var lector = cmd.ExecuteReader();
        while (lector.Read())
        {
            rentabilidad.Add(new Rentabilidad()
            {
                ano = lector.IsDBNull(0) ? 0 : lector.GetInt32(0),
                mes = lector.IsDBNull(1) ? 0 : lector.GetInt32(1),
                mesTexto = lector.IsDBNull(2) ? "N/D" : lector.GetString(2),
                ingresos = lector.IsDBNull(3) ? 0 : Convert.ToDecimal(lector.GetValue(3)),
                gastos = lector.IsDBNull(4) ? 0 : Convert.ToDecimal(lector.GetValue(4)),
                margen = lector.IsDBNull(5) ? 0 : Convert.ToDecimal(lector.GetValue(5)),
                margenPorcentaje = lector.IsDBNull(6) ? 0 : Convert.ToDecimal(lector.GetValue(6)),
                saldo = lector.IsDBNull(7) ? 0 : Convert.ToDecimal(lector.GetValue(7)),
                rentabilidad = lector.IsDBNull(8) ? 0 : Convert.ToDecimal(lector.GetValue(8))
            });
        }
        
        return rentabilidad;
    }
}