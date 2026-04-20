using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Repository;

public interface IRentabilidadRepository
{
    public List<Rentabilidad> ObtenerRentabilidad();
}