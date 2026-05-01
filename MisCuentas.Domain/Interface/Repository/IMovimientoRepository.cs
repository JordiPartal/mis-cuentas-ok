using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Repository;

public interface IMovimientoRepository
{
    public List<Movimiento> ObtenerMovimientos(int? mes, int? ano);
}